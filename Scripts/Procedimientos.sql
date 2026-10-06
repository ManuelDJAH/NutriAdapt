/* =========================================================
   NutriAdapt - Procedimientos almacenados
   Convencion de nombres: {Tabla}_{Accion}Con{Detalle} (sin prefijo sp_).
   Cada procedimiento se crea con su nombre nuevo y se elimina el
   anterior (sp_...). Se puede correr varias veces sin error.
   ========================================================= */
USE NutriAdapt;
GO


/* =========================================================
   Motor de adaptacion de recetas
   Renombrado de sp_GenerarRecetaAdaptada siguiendo la convencion
   {Tabla}_{Accion}Con{Detalle} (sin prefijo sp_).
   factor_grupo = porciones_asignadas / porciones_originales
   ========================================================= */

CREATE OR ALTER PROCEDURE RecetasAdaptadas_GenerarDesdeReceta
    @RecetaId       INT,
    @PacienteId     INT,
    @PlanId         INT = NULL,
    @TiempoComida   VARCHAR(20),
    @GeneradaPor    VARCHAR(20) = 'Nutriologo'
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @RecetaAdaptadaId INT;

    INSERT INTO RecetasAdaptadas (RecetaId, PacienteId, PlanId, TiempoComida, GeneradaPor, FechaGeneracion)
    VALUES (@RecetaId, @PacienteId, @PlanId, @TiempoComida, @GeneradaPor, GETDATE());

    SET @RecetaAdaptadaId = SCOPE_IDENTITY();

    INSERT INTO RecetaAdaptadaDetalle (RecetaAdaptadaId, AlimentoId, CantidadAjustada, UnidadMedida, FactorEscala)
    SELECT
        @RecetaAdaptadaId,
        ri.AlimentoId,
        ri.CantidadOriginal * (pp.CantidadPorciones / ri.PorcionesGrupoOriginal),
        ri.UnidadMedida,
        (pp.CantidadPorciones / ri.PorcionesGrupoOriginal)
    FROM RecetaIngredientes ri
    INNER JOIN Alimentos a ON a.AlimentoId = ri.AlimentoId
    INNER JOIN PlanPorciones pp
        ON pp.GrupoId = a.GrupoId
        AND pp.PlanId = @PlanId
        AND pp.TiempoComida = @TiempoComida
    WHERE ri.RecetaId = @RecetaId;

    SELECT @RecetaAdaptadaId AS RecetaAdaptadaId;
END
GO

DROP PROCEDURE IF EXISTS sp_GenerarRecetaAdaptada;
GO


/* =========================================================
   Alta de paciente: Usuario + Paciente en una sola transaccion
   Renombrado de sp_RegistrarPaciente siguiendo la convencion
   {Tabla}_{Accion}Con{Detalle} (sin prefijo sp_).
   ========================================================= */

CREATE OR ALTER PROCEDURE Pacientes_Registrar
    @NombreCompleto   NVARCHAR(150),
    @Correo           NVARCHAR(150),
    @PasswordHash     NVARCHAR(256),
    @NutriologoId     INT,
    @FechaNacimiento  DATE,
    @Sexo             CHAR(1),
    @ObjetivoGeneral  NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @UsuarioId INT;

        INSERT INTO Usuarios (NombreCompleto, Correo, PasswordHash, RolId)
        VALUES (@NombreCompleto, @Correo, @PasswordHash, 2);

        SET @UsuarioId = SCOPE_IDENTITY();

        INSERT INTO Pacientes (UsuarioId, NutriologoId, FechaNacimiento, Sexo, ObjetivoGeneral)
        VALUES (@UsuarioId, @NutriologoId, @FechaNacimiento, @Sexo, @ObjetivoGeneral);

        COMMIT TRANSACTION;

        SELECT SCOPE_IDENTITY() AS PacienteId;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

DROP PROCEDURE IF EXISTS sp_RegistrarPaciente;
GO


/* =========================================================
   Crear un plan nutricional junto con sus filas de PlanPorciones
   (recibe las porciones por grupo/tiempo de comida como tabla)
   Renombrado de sp_AsignarPlanNutricional siguiendo la convencion
   {Tabla}_{Accion}Con{Detalle} (sin prefijo sp_).
   Requiere el tipo PlanPorcionesTableType (ya existe en la BD).
   ========================================================= */

CREATE OR ALTER PROCEDURE PlanesNutricionales_AsignarConPorciones
    @PacienteId    INT,
    @NutriologoId  INT,
    @FechaInicio   DATE,
    @FechaFin      DATE = NULL,
    @KcalObjetivo  DECIMAL(7,2),
    @Porciones     PlanPorcionesTableType READONLY
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @PlanId INT;

        INSERT INTO PlanesNutricionales (PacienteId, NutriologoId, FechaInicio, FechaFin, KcalObjetivo, Estado)
        VALUES (@PacienteId, @NutriologoId, @FechaInicio, @FechaFin, @KcalObjetivo, 'Activo');

        SET @PlanId = SCOPE_IDENTITY();

        INSERT INTO PlanPorciones (PlanId, GrupoId, TiempoComida, CantidadPorciones)
        SELECT @PlanId, GrupoId, TiempoComida, CantidadPorciones
        FROM @Porciones;

        COMMIT TRANSACTION;

        SELECT @PlanId AS PlanId;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

DROP PROCEDURE IF EXISTS sp_AsignarPlanNutricional;
GO


/* =========================================================
   Cuanto le queda al paciente por grupo/tiempo de comida
   (porciones asignadas menos lo ya consumido via recetas adaptadas)
   Renombrado de sp_ObtenerPorcionesDisponiblesPaciente siguiendo la
   convencion {Tabla}_{Accion}Con{Detalle} (sin prefijo sp_).
   Corregido: antes restaba en cada grupo lo consumido de todos los grupos.
   ========================================================= */

CREATE OR ALTER PROCEDURE PlanPorciones_ObtenerDisponiblesPorPaciente
    @PlanId        INT,
    @TiempoComida  NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ga.GrupoId,
        ga.NombreGrupo,
        pp.CantidadPorciones AS PorcionesAsignadas,
        ISNULL(SUM(ri.PorcionesGrupoOriginal * rad.FactorEscala), 0) AS PorcionesConsumidas,
        pp.CantidadPorciones - ISNULL(SUM(ri.PorcionesGrupoOriginal * rad.FactorEscala), 0) AS PorcionesDisponibles
    FROM PlanPorciones pp
    INNER JOIN GruposAlimenticios ga ON ga.GrupoId = pp.GrupoId
    LEFT JOIN RecetasAdaptadas ra
        ON ra.PlanId = pp.PlanId AND ra.TiempoComida = pp.TiempoComida
    LEFT JOIN RecetaAdaptadaDetalle rad ON rad.RecetaAdaptadaId = ra.RecetaAdaptadaId
    LEFT JOIN Alimentos al ON al.AlimentoId = rad.AlimentoId AND al.GrupoId = ga.GrupoId
    -- al.AlimentoId IS NOT NULL: solo cuenta ingredientes del grupo de esta fila
    LEFT JOIN RecetaIngredientes ri ON ri.AlimentoId = rad.AlimentoId AND ri.RecetaId = ra.RecetaId
        AND al.AlimentoId IS NOT NULL
    WHERE pp.PlanId = @PlanId
      AND pp.TiempoComida = @TiempoComida
    GROUP BY ga.GrupoId, ga.NombreGrupo, pp.CantidadPorciones;
END
GO

DROP PROCEDURE IF EXISTS sp_ObtenerPorcionesDisponiblesPaciente;
GO


/* =========================================================
   Ingredientes de una receta con su grupo, antes de adaptar
   Renombrado de sp_ObtenerDetalleReceta siguiendo la convencion
   {Tabla}_{Accion}Con{Detalle} (sin prefijo sp_).
   ========================================================= */

CREATE OR ALTER PROCEDURE RecetaIngredientes_ObtenerPorReceta
    @RecetaId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        r.RecetaId,
        r.Nombre,
        r.PorcionesRendimientoOriginal,
        ri.RecetaIngredienteId,
        a.AlimentoId,
        a.NombreAlimento,
        ga.NombreGrupo,
        ri.CantidadOriginal,
        ri.UnidadMedida,
        ri.PorcionesGrupoOriginal
    FROM Recetas r
    INNER JOIN RecetaIngredientes ri ON ri.RecetaId = r.RecetaId
    INNER JOIN Alimentos a ON a.AlimentoId = ri.AlimentoId
    INNER JOIN GruposAlimenticios ga ON ga.GrupoId = a.GrupoId
    WHERE r.RecetaId = @RecetaId;
END
GO

DROP PROCEDURE IF EXISTS sp_ObtenerDetalleReceta;
GO


/* =========================================================
   Reporte de historial de recetas adaptadas para el nutriologo
   Renombrado de sp_HistorialRecetasAdaptadasPaciente siguiendo la
   convencion {Tabla}_{Accion}Con{Detalle} (sin prefijo sp_).
   ========================================================= */

CREATE OR ALTER PROCEDURE RecetasAdaptadas_ObtenerHistorialPorPaciente
    @PacienteId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ra.RecetaAdaptadaId,
        r.Nombre AS Receta,
        ra.TiempoComida,
        ra.GeneradaPor,
        ra.FechaGeneracion,
        d.AlimentoId,
        al.NombreAlimento,
        d.CantidadAjustada,
        d.UnidadMedida,
        d.FactorEscala
    FROM RecetasAdaptadas ra
    INNER JOIN Recetas r ON r.RecetaId = ra.RecetaId
    INNER JOIN RecetaAdaptadaDetalle d ON d.RecetaAdaptadaId = ra.RecetaAdaptadaId
    INNER JOIN Alimentos al ON al.AlimentoId = d.AlimentoId
    WHERE ra.PacienteId = @PacienteId
    ORDER BY ra.FechaGeneracion DESC;
END
GO

DROP PROCEDURE IF EXISTS sp_HistorialRecetasAdaptadasPaciente;
GO


/* =========================================================
   AUTENTICACION (usados por UsuarioStore / ASP.NET Core Identity)
   La busqueda por correo depende de la intercalacion CI de la BD:
   Identity manda el correo normalizado en MAYUSCULAS.
   ========================================================= */

CREATE OR ALTER PROCEDURE Usuarios_Crear
    @NombreCompleto  NVARCHAR(150),
    @Correo          NVARCHAR(150),
    @PasswordHash    NVARCHAR(256),
    @RolId           TINYINT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Usuarios (NombreCompleto, Correo, PasswordHash, RolId)
    VALUES (@NombreCompleto, @Correo, @PasswordHash, @RolId);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS UsuarioId;
END
GO

CREATE OR ALTER PROCEDURE Usuarios_ObtenerPorCorreo
    @Correo NVARCHAR(150)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT UsuarioId, NombreCompleto, Correo, PasswordHash, RolId, FechaCreacion
    FROM Usuarios
    WHERE Correo = @Correo;
END
GO

CREATE OR ALTER PROCEDURE Usuarios_ObtenerPorId
    @UsuarioId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT UsuarioId, NombreCompleto, Correo, PasswordHash, RolId, FechaCreacion
    FROM Usuarios
    WHERE UsuarioId = @UsuarioId;
END
GO

CREATE OR ALTER PROCEDURE Usuarios_Actualizar
    @UsuarioId       INT,
    @NombreCompleto  NVARCHAR(150),
    @Correo          NVARCHAR(150),
    @PasswordHash    NVARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Usuarios
    SET NombreCompleto = @NombreCompleto,
        Correo = @Correo,
        PasswordHash = @PasswordHash
    WHERE UsuarioId = @UsuarioId;
END
GO

CREATE OR ALTER PROCEDURE Usuarios_Borrar
    @UsuarioId INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Usuarios WHERE UsuarioId = @UsuarioId;
END
GO

-- Perfil de nutriologo para un usuario ya creado (registro publico)
CREATE OR ALTER PROCEDURE Nutriologos_Crear
    @UsuarioId          INT,
    @CedulaProfesional  NVARCHAR(20),
    @Especialidad       NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Nutriologos (UsuarioId, CedulaProfesional, Especialidad)
    VALUES (@UsuarioId, @CedulaProfesional, @Especialidad);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS NutriologoId;
END
GO
