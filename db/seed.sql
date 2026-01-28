-- =====================================================================
-- Sistema de Gestión de Bajas Técnicas
-- Script de Seed para Testing End-to-End de Reportes
-- =====================================================================
-- Autor: [Tu nombre]
-- Fecha: Enero 2026
-- Versión: 1.0
--
-- OBJETIVO:
-- Proporcionar datos estables y reproducibles para validar los 7 reportes
-- del sistema sin depender de Docker ni infraestructura externa.
--
-- ESCENARIOS CUBIERTOS:
-- - Reporte 1: Bajas en diferentes rangos de fechas y destinos
-- - Reporte 2: Historial completo de mantenimientos
-- - Reporte 3: Traslados entre secciones vs dentro de la misma sección
-- - Reporte 4: Correlación técnico-rendimiento con fallos irreparables
-- - Reporte 5: Equipos con >3 mantenimientos en el último año
-- - Reporte 6: Rendimiento de técnicos con múltiples valoraciones
-- - Reporte 7: Equipos enviados a departamento específico (bajas + traslados)
-- =====================================================================

-- =====================================================================
-- PASO 1: LIMPIEZA DE DATOS EXISTENTES (orden inverso de dependencias)
-- =====================================================================

SET FOREIGN_KEY_CHECKS = 0;

-- Eliminar datos transaccionales primero
DELETE FROM TransferRequests;
DELETE FROM Transfers;
DELETE FROM EquipmentDecommissions;
DELETE FROM Assessments;
DELETE FROM Maintenances;

-- Eliminar entidades principales
DELETE FROM Equipments;
DELETE FROM EquipmentTypes;

-- Eliminar usuarios (en orden de herencia: más específico primero)
DELETE FROM Responsibles;
DELETE FROM Employees;
DELETE FROM Technicals;
DELETE FROM Directors;
DELETE FROM Users;

-- Eliminar estructura organizacional
DELETE FROM Departments;
DELETE FROM Sections;

SET FOREIGN_KEY_CHECKS = 1;

-- =====================================================================
-- PASO 2: INSERCIÓN DE DATOS (orden correcto respetando foreign keys)
-- =====================================================================

-- ---------------------------------------------------------------------
-- 2.1 SECCIONES (3 secciones para probar traslados entre/dentro)
-- ---------------------------------------------------------------------

INSERT INTO Sections (Id, Name) VALUES
-- Sección 1: Tecnología
('aaaaaaaa-0001-0000-0000-000000000001', 'Tecnología e Infraestructura'),

-- Sección 2: Recursos Humanos
('aaaaaaaa-0002-0000-0000-000000000002', 'Recursos Humanos'),

-- Sección 3: Finanzas
('aaaaaaaa-0003-0000-0000-000000000003', 'Finanzas y Administración');

-- ---------------------------------------------------------------------
-- 2.2 DEPARTAMENTOS (6 departamentos: 2 por sección)
-- ---------------------------------------------------------------------

INSERT INTO Departments (Id, Name, SectionId) VALUES
-- Departamentos de Tecnología (Sección 1)
('bbbbbbbb-0001-0000-0000-000000000001', 'Desarrollo de Software', 'aaaaaaaa-0001-0000-0000-000000000001'),
('bbbbbbbb-0002-0000-0000-000000000002', 'Infraestructura y Redes', 'aaaaaaaa-0001-0000-0000-000000000001'),

-- Departamentos de Recursos Humanos (Sección 2)
('bbbbbbbb-0003-0000-0000-000000000003', 'Gestión de Talento', 'aaaaaaaa-0002-0000-0000-000000000002'),
('bbbbbbbb-0004-0000-0000-000000000004', 'Capacitación', 'aaaaaaaa-0002-0000-0000-000000000002'),

-- Departamentos de Finanzas (Sección 3)
('bbbbbbbb-0005-0000-0000-000000000005', 'Contabilidad', 'aaaaaaaa-0003-0000-0000-000000000003'),
('bbbbbbbb-0006-0000-0000-000000000006', 'Auditoría', 'aaaaaaaa-0003-0000-0000-000000000003');

-- ---------------------------------------------------------------------
-- 2.3 TIPOS DE EQUIPO (3 tipos)
-- ---------------------------------------------------------------------

INSERT INTO EquipmentTypes (Id, Name) VALUES
('cccccccc-0001-0000-0000-000000000001', 'Informático'),
('cccccccc-0002-0000-0000-000000000002', 'Comunicaciones'),
('cccccccc-0003-0000-0000-000000000003', 'Eléctrico');

-- ---------------------------------------------------------------------
-- 2.4 USUARIOS: TÉCNICOS (5 técnicos con diferentes perfiles)
-- ---------------------------------------------------------------------
-- Nota: La tabla Technicals hereda de Users, así que primero insertamos en Users
-- y luego en Technicals con el mismo Id.

-- Técnico 1: Experto en hardware, alta experiencia
INSERT INTO Users (Id, Name, Email, PasswordHash, RoleId) VALUES
('dddddddd-0001-0000-0000-000000000001', 'Juan Pérez', 'juan.perez@company.com', 'hash_password_123', 3);

INSERT INTO Technicals (Id, Experience, Specialty) VALUES
('dddddddd-0001-0000-0000-000000000001', 10, 'Hardware y Reparación');

-- Técnico 2: Especialista en redes, experiencia media
INSERT INTO Users (Id, Name, Email, PasswordHash, RoleId) VALUES
('dddddddd-0002-0000-0000-000000000002', 'María García', 'maria.garcia@company.com', 'hash_password_456', 3);

INSERT INTO Technicals (Id, Experience, Specialty) VALUES
('dddddddd-0002-0000-0000-000000000002', 5, 'Redes y Comunicaciones');

-- Técnico 3: Junior en software, poca experiencia (para correlación negativa)
INSERT INTO Users (Id, Name, Email, PasswordHash, RoleId) VALUES
('dddddddd-0003-0000-0000-000000000003', 'Carlos López', 'carlos.lopez@company.com', 'hash_password_789', 3);

INSERT INTO Technicals (Id, Experience, Specialty) VALUES
('dddddddd-0003-0000-0000-000000000003', 2, 'Software y Sistemas');

-- Técnico 4: Experto en eléctricos
INSERT INTO Users (Id, Name, Email, PasswordHash, RoleId) VALUES
('dddddddd-0004-0000-0000-000000000004', 'Ana Martínez', 'ana.martinez@company.com', 'hash_password_abc', 3);

INSERT INTO Technicals (Id, Experience, Specialty) VALUES
('dddddddd-0004-0000-0000-000000000004', 8, 'Sistemas Eléctricos');

-- Técnico 5: Generalista
INSERT INTO Users (Id, Name, Email, PasswordHash, RoleId) VALUES
('dddddddd-0005-0000-0000-000000000005', 'Luis Fernández', 'luis.fernandez@company.com', 'hash_password_def', 3);

INSERT INTO Technicals (Id, Experience, Specialty) VALUES
('dddddddd-0005-0000-0000-000000000005', 6, 'Mantenimiento General');

-- ---------------------------------------------------------------------
-- 2.5 USUARIOS: DIRECTORES (3 directores para valoraciones)
-- ---------------------------------------------------------------------

INSERT INTO Users (Id, Name, Email, PasswordHash, RoleId) VALUES
('eeeeeeee-0001-0000-0000-000000000001', 'Roberto Sánchez', 'roberto.sanchez@company.com', 'hash_director_1', 2);

INSERT INTO Directors (Id) VALUES
('eeeeeeee-0001-0000-0000-000000000001');

INSERT INTO Users (Id, Name, Email, PasswordHash, RoleId) VALUES
('eeeeeeee-0002-0000-0000-000000000002', 'Patricia Gómez', 'patricia.gomez@company.com', 'hash_director_2', 2);

INSERT INTO Directors (Id) VALUES
('eeeeeeee-0002-0000-0000-000000000002');

INSERT INTO Users (Id, Name, Email, PasswordHash, RoleId) VALUES
('eeeeeeee-0003-0000-0000-000000000003', 'Miguel Herrera', 'miguel.herrera@company.com', 'hash_director_3', 2);

INSERT INTO Directors (Id) VALUES
('eeeeeeee-0003-0000-0000-000000000003');

-- ---------------------------------------------------------------------
-- 2.6 USUARIOS: RESPONSABLES (5 responsables para traslados)
-- ---------------------------------------------------------------------
-- Nota: Responsibles hereda de Employee, que hereda de User

-- Responsable 1: Desarrollo
INSERT INTO Users (Id, Name, Email, PasswordHash, RoleId) VALUES
('ffffffff-0001-0000-0000-000000000001', 'Laura Torres', 'laura.torres@company.com', 'hash_resp_1', 5);

INSERT INTO Employees (Id, DepartmentId) VALUES
('ffffffff-0001-0000-0000-000000000001', 'bbbbbbbb-0001-0000-0000-000000000001');

INSERT INTO Responsibles (Id) VALUES
('ffffffff-0001-0000-0000-000000000001');

-- Responsable 2: Infraestructura
INSERT INTO Users (Id, Name, Email, PasswordHash, RoleId) VALUES
('ffffffff-0002-0000-0000-000000000002', 'Jorge Díaz', 'jorge.diaz@company.com', 'hash_resp_2', 5);

INSERT INTO Employees (Id, DepartmentId) VALUES
('ffffffff-0002-0000-0000-000000000002', 'bbbbbbbb-0002-0000-0000-000000000002');

INSERT INTO Responsibles (Id) VALUES
('ffffffff-0002-0000-0000-000000000002');

-- Responsable 3: Gestión de Talento
INSERT INTO Users (Id, Name, Email, PasswordHash, RoleId) VALUES
('ffffffff-0003-0000-0000-000000000003', 'Carmen Ruiz', 'carmen.ruiz@company.com', 'hash_resp_3', 5);

INSERT INTO Employees (Id, DepartmentId) VALUES
('ffffffff-0003-0000-0000-000000000003', 'bbbbbbbb-0003-0000-0000-000000000003');

INSERT INTO Responsibles (Id) VALUES
('ffffffff-0003-0000-0000-000000000003');

-- Responsable 4: Capacitación
INSERT INTO Users (Id, Name, Email, PasswordHash, RoleId) VALUES
('ffffffff-0004-0000-0000-000000000004', 'Pedro Morales', 'pedro.morales@company.com', 'hash_resp_4', 5);

INSERT INTO Employees (Id, DepartmentId) VALUES
('ffffffff-0004-0000-0000-000000000004', 'bbbbbbbb-0004-0000-0000-000000000004');

INSERT INTO Responsibles (Id) VALUES
('ffffffff-0004-0000-0000-000000000004');

-- Responsable 5: Contabilidad
INSERT INTO Users (Id, Name, Email, PasswordHash, RoleId) VALUES
('ffffffff-0005-0000-0000-000000000005', 'Sofía Ramírez', 'sofia.ramirez@company.com', 'hash_resp_5', 5);

INSERT INTO Employees (Id, DepartmentId) VALUES
('ffffffff-0005-0000-0000-000000000005', 'bbbbbbbb-0005-0000-0000-000000000005');

INSERT INTO Responsibles (Id) VALUES
('ffffffff-0005-0000-0000-000000000005');

-- ---------------------------------------------------------------------
-- 2.7 USUARIOS: RECEPTORES (2 receptores para bajas)
-- ---------------------------------------------------------------------

INSERT INTO Users (Id, Name, Email, PasswordHash, RoleId) VALUES
('7aaaaaaa-0001-0000-0000-000000000001', 'Ricardo Vega', 'ricardo.vega@company.com', 'hash_receptor_1', 6);

INSERT INTO Employees (Id, DepartmentId) VALUES
('7aaaaaaa-0001-0000-0000-000000000001', 'bbbbbbbb-0001-0000-0000-000000000001');

INSERT INTO Users (Id, Name, Email, PasswordHash, RoleId) VALUES
('7aaaaaaa-0002-0000-0000-000000000002', 'Valeria Castro', 'valeria.castro@company.com', 'hash_receptor_2', 6);

INSERT INTO Employees (Id, DepartmentId) VALUES
('7aaaaaaa-0002-0000-0000-000000000002', 'bbbbbbbb-0003-0000-0000-000000000003');

-- ---------------------------------------------------------------------
-- 2.8 EQUIPOS (15 equipos en diferentes estados y ubicaciones)
-- ---------------------------------------------------------------------
-- Estados: 1=Operative, 2=UnderMaintenance, 3=Decommissioned, 4=Disposed
-- Ubicaciones: 1=Department, 2=Warehouse, 3=Disposal

-- ===== EQUIPOS PARA REPORTE 1 (Bajas último año) =====

-- Equipo 1: Dado de baja hace 6 meses por fallo técnico irreparable → Disposal
INSERT INTO Equipments (Id, Name, StateId, LocationTypeId, AcquisitionDate, EquipmentTypeId, DepartmentId) VALUES
('8bbbbbbb-0001-0000-0000-000000000001', 'Servidor Principal A', 4, 3, DATE_SUB(UTC_TIMESTAMP(), INTERVAL 3 YEAR), 'cccccccc-0001-0000-0000-000000000001', NULL);

-- Equipo 2: Dado de baja hace 2 meses por obsolescencia → Warehouse
INSERT INTO Equipments (Id, Name, StateId, LocationTypeId, AcquisitionDate, EquipmentTypeId, DepartmentId) VALUES
('8bbbbbbb-0002-0000-0000-000000000002', 'Switch Core B', 3, 2, DATE_SUB(UTC_TIMESTAMP(), INTERVAL 5 YEAR), 'cccccccc-0002-0000-0000-000000000002', NULL);

-- Equipo 3: Dado de baja hace 11 meses → Department (transferido a otro dpto)
INSERT INTO Equipments (Id, Name, StateId, LocationTypeId, AcquisitionDate, EquipmentTypeId, DepartmentId) VALUES
('8bbbbbbb-0003-0000-0000-000000000003', 'Laptop Dell XPS 15', 1, 1, DATE_SUB(UTC_TIMESTAMP(), INTERVAL 2 YEAR), 'cccccccc-0001-0000-0000-000000000001', 'bbbbbbbb-0003-0000-0000-000000000003');

-- Equipo 4: Baja hace 14 meses (FUERA del rango del último año) → No aparece en R1
INSERT INTO Equipments (Id, Name, StateId, LocationTypeId, AcquisitionDate, EquipmentTypeId, DepartmentId) VALUES
('8bbbbbbb-0004-0000-0000-000000000004', 'Router Cisco antiguo', 4, 3, DATE_SUB(UTC_TIMESTAMP(), INTERVAL 6 YEAR), 'cccccccc-0002-0000-0000-000000000002', NULL);

-- ===== EQUIPOS PARA REPORTE 4 (Correlación técnico-rendimiento) =====

-- Equipo 5: Fallo irreparable, alto costo mantenimiento, técnico con baja valoración
INSERT INTO Equipments (Id, Name, StateId, LocationTypeId, AcquisitionDate, EquipmentTypeId, DepartmentId) VALUES
('8bbbbbbb-0005-0000-0000-000000000005', 'Servidor Database C', 4, 3, DATE_SUB(UTC_TIMESTAMP(), INTERVAL 2 YEAR), 'cccccccc-0001-0000-0000-000000000001', NULL);

-- Equipo 6: Fallo irreparable, múltiples mantenimientos costosos
INSERT INTO Equipments (Id, Name, StateId, LocationTypeId, AcquisitionDate, EquipmentTypeId, DepartmentId) VALUES
('8bbbbbbb-0006-0000-0000-000000000006', 'UPS Central', 4, 3, DATE_SUB(UTC_TIMESTAMP(), INTERVAL 18 MONTH), 'cccccccc-0003-0000-0000-000000000003', NULL);

-- ===== EQUIPOS PARA REPORTE 5 (>3 mantenimientos último año) =====

-- Equipo 7: 5 mantenimientos en el último año → SÍ aparece en R5
INSERT INTO Equipments (Id, Name, StateId, LocationTypeId, AcquisitionDate, EquipmentTypeId, DepartmentId) VALUES
('8bbbbbbb-0007-0000-0000-000000000007', 'Impresora HP LaserJet', 1, 1, DATE_SUB(UTC_TIMESTAMP(), INTERVAL 3 YEAR), 'cccccccc-0001-0000-0000-000000000001', 'bbbbbbbb-0001-0000-0000-000000000001');

-- Equipo 8: 4 mantenimientos en el último año → SÍ aparece en R5
INSERT INTO Equipments (Id, Name, StateId, LocationTypeId, AcquisitionDate, EquipmentTypeId, DepartmentId) VALUES
('8bbbbbbb-0008-0000-0000-000000000008', 'Scanner Epson', 1, 1, DATE_SUB(UTC_TIMESTAMP(), INTERVAL 2 YEAR), 'cccccccc-0001-0000-0000-000000000001', 'bbbbbbbb-0002-0000-0000-000000000002');

-- Equipo 9: 2 mantenimientos último año → NO aparece en R5
INSERT INTO Equipments (Id, Name, StateId, LocationTypeId, AcquisitionDate, EquipmentTypeId, DepartmentId) VALUES
('8bbbbbbb-0009-0000-0000-000000000009', 'Proyector Benq', 1, 1, DATE_SUB(UTC_TIMESTAMP(), INTERVAL 1 YEAR), 'cccccccc-0001-0000-0000-000000000001', 'bbbbbbbb-0004-0000-0000-000000000004');

-- ===== EQUIPOS PARA REPORTE 2 (Historial mantenimientos) =====

-- Equipo 10: Múltiples mantenimientos de diferentes tipos
INSERT INTO Equipments (Id, Name, StateId, LocationTypeId, AcquisitionDate, EquipmentTypeId, DepartmentId) VALUES
('8bbbbbbb-0010-0000-0000-000000000010', 'Servidor Web D', 1, 1, DATE_SUB(UTC_TIMESTAMP(), INTERVAL 4 YEAR), 'cccccccc-0001-0000-0000-000000000001', 'bbbbbbbb-0001-0000-0000-000000000001');

-- ===== EQUIPOS PARA REPORTE 3 y 7 (Traslados) =====

-- Equipo 11: Trasladado ENTRE secciones → SÍ aparece en R3
INSERT INTO Equipments (Id, Name, StateId, LocationTypeId, AcquisitionDate, EquipmentTypeId, DepartmentId) VALUES
('8bbbbbbb-0011-0000-0000-000000000011', 'Laptop Lenovo ThinkPad', 1, 1, DATE_SUB(UTC_TIMESTAMP(), INTERVAL 1 YEAR), 'cccccccc-0001-0000-0000-000000000001', 'bbbbbbbb-0003-0000-0000-000000000003');

-- Equipo 12: Trasladado DENTRO de la misma sección → NO aparece en R3
INSERT INTO Equipments (Id, Name, StateId, LocationTypeId, AcquisitionDate, EquipmentTypeId, DepartmentId) VALUES
('8bbbbbbb-0012-0000-0000-000000000012', 'Desktop HP EliteDesk', 1, 1, DATE_SUB(UTC_TIMESTAMP(), INTERVAL 1 YEAR), 'cccccccc-0001-0000-0000-000000000001', 'bbbbbbbb-0002-0000-0000-000000000002');

-- ===== EQUIPOS PARA REPORTE 7 (Enviados a departamento específico) =====

-- Equipo 13: Dado de baja con destino = Departamento Desarrollo → SÍ aparece en R7
INSERT INTO Equipments (Id, Name, StateId, LocationTypeId, AcquisitionDate, EquipmentTypeId, DepartmentId) VALUES
('8bbbbbbb-0013-0000-0000-000000000013', 'Monitor Dell 27"', 1, 1, DATE_SUB(UTC_TIMESTAMP(), INTERVAL 2 YEAR), 'cccccccc-0001-0000-0000-000000000001', 'bbbbbbbb-0001-0000-0000-000000000001');

-- Equipo 14: Trasladado a Departamento Desarrollo → SÍ aparece en R7
INSERT INTO Equipments (Id, Name, StateId, LocationTypeId, AcquisitionDate, EquipmentTypeId, DepartmentId) VALUES
('8bbbbbbb-0014-0000-0000-000000000014', 'Teclado Logitech MX', 1, 1, DATE_SUB(UTC_TIMESTAMP(), INTERVAL 1 YEAR), 'cccccccc-0001-0000-0000-000000000001', 'bbbbbbbb-0001-0000-0000-000000000001');

-- Equipo 15: En mantenimiento actualmente
INSERT INTO Equipments (Id, Name, StateId, LocationTypeId, AcquisitionDate, EquipmentTypeId, DepartmentId) VALUES
('8bbbbbbb-0015-0000-0000-000000000015', 'Switch Netgear GS724T', 2, 1, DATE_SUB(UTC_TIMESTAMP(), INTERVAL 6 MONTH), 'cccccccc-0002-0000-0000-000000000002', 'bbbbbbbb-0002-0000-0000-000000000002');

-- ---------------------------------------------------------------------
-- 2.9 MANTENIMIENTOS (30+ registros estratégicos)
-- ---------------------------------------------------------------------
-- Tipos: 1=Preventive, 2=Corrective, 3=Predictive, 4=Emergency
-- Estados: 1=InProgress, 2=Completed

-- ===== MANTENIMIENTOS PARA REPORTE 2 (Equipo 10 - Servidor Web D) =====

-- Mantenimiento 1: Preventivo hace 3 años
INSERT INTO Maintenances (Id, EquipmentId, TechnicalId, MaintenanceDate, EndDate, StatusId, MaintenanceTypeId, Cost) VALUES
('9ccccccc-0001-0000-0000-000000000001', '8bbbbbbb-0010-0000-0000-000000000010', 'dddddddd-0001-0000-0000-000000000001', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 3 YEAR), DATE_SUB(UTC_TIMESTAMP(), INTERVAL 3 YEAR), 2, 1, 150.00);

-- Mantenimiento 2: Correctivo hace 2 años
INSERT INTO Maintenances (Id, EquipmentId, TechnicalId, MaintenanceDate, EndDate, StatusId, MaintenanceTypeId, Cost) VALUES
('9ccccccc-0002-0000-0000-000000000002', '8bbbbbbb-0010-0000-0000-000000000010', 'dddddddd-0002-0000-0000-000000000002', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 2 YEAR), DATE_SUB(UTC_TIMESTAMP(), INTERVAL 2 YEAR), 2, 2, 300.00);

-- Mantenimiento 3: Predictivo hace 1 año
INSERT INTO Maintenances (Id, EquipmentId, TechnicalId, MaintenanceDate, EndDate, StatusId, MaintenanceTypeId, Cost) VALUES
('9ccccccc-0003-0000-0000-000000000003', '8bbbbbbb-0010-0000-0000-000000000010', 'dddddddd-0003-0000-0000-000000000003', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 1 YEAR), DATE_SUB(UTC_TIMESTAMP(), INTERVAL 1 YEAR), 2, 3, 200.00);

-- Mantenimiento 4: Emergencia hace 6 meses
INSERT INTO Maintenances (Id, EquipmentId, TechnicalId, MaintenanceDate, EndDate, StatusId, MaintenanceTypeId, Cost) VALUES
('9ccccccc-0004-0000-0000-000000000004', '8bbbbbbb-0010-0000-0000-000000000010', 'dddddddd-0004-0000-0000-000000000004', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 6 MONTH), DATE_SUB(UTC_TIMESTAMP(), INTERVAL 6 MONTH), 2, 4, 500.00);

-- ===== MANTENIMIENTOS PARA REPORTE 5 (Equipo 7 - 5 mantenimientos) =====

INSERT INTO Maintenances (Id, EquipmentId, TechnicalId, MaintenanceDate, EndDate, StatusId, MaintenanceTypeId, Cost) VALUES
('9ccccccc-0005-0000-0000-000000000005', '8bbbbbbb-0007-0000-0000-000000000007', 'dddddddd-0001-0000-0000-000000000001', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 11 MONTH), DATE_SUB(UTC_TIMESTAMP(), INTERVAL 11 MONTH), 2, 2, 120.00),
('9ccccccc-0006-0000-0000-000000000006', '8bbbbbbb-0007-0000-0000-000000000007', 'dddddddd-0001-0000-0000-000000000001', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 9 MONTH), DATE_SUB(UTC_TIMESTAMP(), INTERVAL 9 MONTH), 2, 2, 130.00),
('9ccccccc-0007-0000-0000-000000000007', '8bbbbbbb-0007-0000-0000-000000000007', 'dddddddd-0001-0000-0000-000000000001', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 7 MONTH), DATE_SUB(UTC_TIMESTAMP(), INTERVAL 7 MONTH), 2, 2, 140.00),
('9ccccccc-0008-0000-0000-000000000008', '8bbbbbbb-0007-0000-0000-000000000007', 'dddddddd-0001-0000-0000-000000000001', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 5 MONTH), DATE_SUB(UTC_TIMESTAMP(), INTERVAL 5 MONTH), 2, 2, 150.00),
('9ccccccc-0009-0000-0000-000000000009', '8bbbbbbb-0007-0000-0000-000000000007', 'dddddddd-0001-0000-0000-000000000001', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 2 MONTH), DATE_SUB(UTC_TIMESTAMP(), INTERVAL 2 MONTH), 2, 2, 160.00);

-- ===== MANTENIMIENTOS PARA REPORTE 5 (Equipo 8 - 4 mantenimientos) =====

INSERT INTO Maintenances (Id, EquipmentId, TechnicalId, MaintenanceDate, EndDate, StatusId, MaintenanceTypeId, Cost) VALUES
('9ccccccc-0010-0000-0000-000000000010', '8bbbbbbb-0008-0000-0000-000000000008', 'dddddddd-0002-0000-0000-000000000002', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 10 MONTH), DATE_SUB(UTC_TIMESTAMP(), INTERVAL 10 MONTH), 2, 1, 80.00),
('9ccccccc-0011-0000-0000-000000000011', '8bbbbbbb-0008-0000-0000-000000000008', 'dddddddd-0002-0000-0000-000000000002', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 7 MONTH), DATE_SUB(UTC_TIMESTAMP(), INTERVAL 7 MONTH), 2, 2, 110.00),
('9ccccccc-0012-0000-0000-000000000012', '8bbbbbbb-0008-0000-0000-000000000008', 'dddddddd-0002-0000-0000-000000000002', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 4 MONTH), DATE_SUB(UTC_TIMESTAMP(), INTERVAL 4 MONTH), 2, 2, 120.00),
('9ccccccc-0013-0000-0000-000000000013', '8bbbbbbb-0008-0000-0000-000000000008', 'dddddddd-0002-0000-0000-000000000002', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 1 MONTH), DATE_SUB(UTC_TIMESTAMP(), INTERVAL 1 MONTH), 2, 4, 250.00);

-- ===== MANTENIMIENTOS PARA REPORTE 5 (Equipo 9 - solo 2 mantenimientos) =====

INSERT INTO Maintenances (Id, EquipmentId, TechnicalId, MaintenanceDate, EndDate, StatusId, MaintenanceTypeId, Cost) VALUES
('9ccccccc-0014-0000-0000-000000000014', '8bbbbbbb-0009-0000-0000-000000000009', 'dddddddd-0003-0000-0000-000000000003', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 8 MONTH), DATE_SUB(UTC_TIMESTAMP(), INTERVAL 8 MONTH), 2, 1, 60.00),
('9ccccccc-0015-0000-0000-000000000015', '8bbbbbbb-0009-0000-0000-000000000009', 'dddddddd-0003-0000-0000-000000000003', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 3 MONTH), DATE_SUB(UTC_TIMESTAMP(), INTERVAL 3 MONTH), 2, 2, 90.00);

-- ===== MANTENIMIENTOS PARA REPORTE 4 (Equipos con fallo irreparable) =====

-- Equipo 5: Múltiples mantenimientos costosos antes del fallo (técnico 3 - junior, baja valoración)
INSERT INTO Maintenances (Id, EquipmentId, TechnicalId, MaintenanceDate, EndDate, StatusId, MaintenanceTypeId, Cost) VALUES
('9ccccccc-0016-0000-0000-000000000016', '8bbbbbbb-0005-0000-0000-000000000005', 'dddddddd-0003-0000-0000-000000000003', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 18 MONTH), DATE_SUB(UTC_TIMESTAMP(), INTERVAL 18 MONTH), 2, 2, 800.00),
('9ccccccc-0017-0000-0000-000000000017', '8bbbbbbb-0005-0000-0000-000000000005', 'dddddddd-0003-0000-0000-000000000003', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 15 MONTH), DATE_SUB(UTC_TIMESTAMP(), INTERVAL 15 MONTH), 2, 2, 900.00),
('9ccccccc-0018-0000-0000-000000000018', '8bbbbbbb-0005-0000-0000-000000000005', 'dddddddd-0003-0000-0000-000000000003', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 12 MONTH), DATE_SUB(UTC_TIMESTAMP(), INTERVAL 12 MONTH), 2, 4, 1200.00),
('9ccccccc-0019-0000-0000-000000000019', '8bbbbbbb-0005-0000-0000-000000000005', 'dddddddd-0003-0000-0000-000000000003', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 8 MONTH), DATE_SUB(UTC_TIMESTAMP(), INTERVAL 8 MONTH), 2, 4, 1500.00);

-- Equipo 6: Mantenimientos costosos antes del fallo (técnico 5)
INSERT INTO Maintenances (Id, EquipmentId, TechnicalId, MaintenanceDate, EndDate, StatusId, MaintenanceTypeId, Cost) VALUES
('9ccccccc-0020-0000-0000-000000000020', '8bbbbbbb-0006-0000-0000-000000000006', 'dddddddd-0005-0000-0000-000000000005', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 14 MONTH), DATE_SUB(UTC_TIMESTAMP(), INTERVAL 14 MONTH), 2, 2, 600.00),
('9ccccccc-0021-0000-0000-000000000021', '8bbbbbbb-0006-0000-0000-000000000006', 'dddddddd-0005-0000-0000-000000000005', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 10 MONTH), DATE_SUB(UTC_TIMESTAMP(), INTERVAL 10 MONTH), 2, 2, 700.00),
('9ccccccc-0022-0000-0000-000000000022', '8bbbbbbb-0006-0000-0000-000000000006', 'dddddddd-0005-0000-0000-000000000005', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 7 MONTH), DATE_SUB(UTC_TIMESTAMP(), INTERVAL 7 MONTH), 2, 4, 1000.00);

-- ===== MANTENIMIENTO EN PROGRESO (Equipo 15) =====

INSERT INTO Maintenances (Id, EquipmentId, TechnicalId, MaintenanceDate, EndDate, StatusId, MaintenanceTypeId, Cost) VALUES
('9ccccccc-0023-0000-0000-000000000023', '8bbbbbbb-0015-0000-0000-000000000015', 'dddddddd-0004-0000-0000-000000000004', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 1 DAY), NULL, 1, 2, 200.00);

-- ===== MANTENIMIENTOS ADICIONALES PARA REPORTE 6 (Rendimiento técnicos) =====

-- Técnico 1: Más mantenimientos (alta productividad)
INSERT INTO Maintenances (Id, EquipmentId, TechnicalId, MaintenanceDate, EndDate, StatusId, MaintenanceTypeId, Cost) VALUES
('9ccccccc-0024-0000-0000-000000000024', '8bbbbbbb-0001-0000-0000-000000000001', 'dddddddd-0001-0000-0000-000000000001', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 30 MONTH), DATE_SUB(UTC_TIMESTAMP(), INTERVAL 30 MONTH), 2, 1, 100.00),
('9ccccccc-0025-0000-0000-000000000025', '8bbbbbbb-0002-0000-0000-000000000002', 'dddddddd-0001-0000-0000-000000000001', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 28 MONTH), DATE_SUB(UTC_TIMESTAMP(), INTERVAL 28 MONTH), 2, 1, 110.00);

-- Técnico 2: Cantidad media de mantenimientos
INSERT INTO Maintenances (Id, EquipmentId, TechnicalId, MaintenanceDate, EndDate, StatusId, MaintenanceTypeId, Cost) VALUES
('9ccccccc-0026-0000-0000-000000000026', '8bbbbbbb-0003-0000-0000-000000000003', 'dddddddd-0002-0000-0000-000000000002', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 13 MONTH), DATE_SUB(UTC_TIMESTAMP(), INTERVAL 13 MONTH), 2, 1, 90.00),
('9ccccccc-0027-0000-0000-000000000027', '8bbbbbbb-0004-0000-0000-000000000004', 'dddddddd-0002-0000-0000-000000000002', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 16 MONTH), DATE_SUB(UTC_TIMESTAMP(), INTERVAL 16 MONTH), 2, 1, 95.00);

-- ---------------------------------------------------------------------
-- 2.10 VALORACIONES DE TÉCNICOS (15 assessments para Reporte 6)
-- ---------------------------------------------------------------------
-- Score: valor entre 0 y 100

-- ===== TÉCNICO 1 (Juan Pérez): Alta valoración promedio (85) =====

INSERT INTO Assessments (Id, TechnicalId, DirectorId, Score, Comment, AssessmentDate) VALUES
('addddddd-0001-0000-0000-000000000001', 'dddddddd-0001-0000-0000-000000000001', 'eeeeeeee-0001-0000-0000-000000000001', 90.00, 'Excelente trabajo en reparación de hardware. Muy eficiente.', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 6 MONTH)),
('addddddd-0002-0000-0000-000000000002', 'dddddddd-0001-0000-0000-000000000001', 'eeeeeeee-0002-0000-0000-000000000002', 85.00, 'Buen desempeño, cumple con los plazos.', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 3 MONTH)),
('addddddd-0003-0000-0000-000000000003', 'dddddddd-0001-0000-0000-000000000001', 'eeeeeeee-0003-0000-0000-000000000003', 80.00, 'Sólido rendimiento, puede mejorar en documentación.', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 1 MONTH));

-- ===== TÉCNICO 2 (María García): Valoración media (75) =====

INSERT INTO Assessments (Id, TechnicalId, DirectorId, Score, Comment, AssessmentDate) VALUES
('addddddd-0004-0000-0000-000000000004', 'dddddddd-0002-0000-0000-000000000002', 'eeeeeeee-0001-0000-0000-000000000001', 78.00, 'Buen conocimiento de redes, podría ser más proactiva.', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 5 MONTH)),
('addddddd-0005-0000-0000-000000000005', 'dddddddd-0002-0000-0000-000000000002', 'eeeeeeee-0002-0000-0000-000000000002', 72.00, 'Cumple con las tareas asignadas, falta iniciativa.', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 2 MONTH)),
('addddddd-0006-0000-0000-000000000006', 'dddddddd-0002-0000-0000-000000000002', 'eeeeeeee-0003-0000-0000-000000000003', 75.00, 'Desempeño aceptable, necesita mejorar tiempos de respuesta.', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 1 WEEK));

-- ===== TÉCNICO 3 (Carlos López): Baja valoración (45) - Para Reporte 4 =====

INSERT INTO Assessments (Id, TechnicalId, DirectorId, Score, Comment, AssessmentDate) VALUES
('addddddd-0007-0000-0000-000000000007', 'dddddddd-0003-0000-0000-000000000003', 'eeeeeeee-0001-0000-0000-000000000001', 50.00, 'Necesita mejorar habilidades técnicas urgentemente.', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 7 MONTH)),
('addddddd-0008-0000-0000-000000000008', 'dddddddd-0003-0000-0000-000000000003', 'eeeeeeee-0002-0000-0000-000000000002', 40.00, 'Demasiados errores en diagnósticos. Requiere capacitación.', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 4 MONTH)),
('addddddd-0009-0000-0000-000000000009', 'dddddddd-0003-0000-0000-000000000003', 'eeeeeeee-0003-0000-0000-000000000003', 45.00, 'Bajo rendimiento, equipos fallan prematuramente.', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 1 MONTH));

-- ===== TÉCNICO 4 (Ana Martínez): Alta valoración (88) =====

INSERT INTO Assessments (Id, TechnicalId, DirectorId, Score, Comment, AssessmentDate) VALUES
('addddddd-0010-0000-0000-000000000010', 'dddddddd-0004-0000-0000-000000000004', 'eeeeeeee-0001-0000-0000-000000000001', 92.00, 'Excelente en sistemas eléctricos. Muy recomendada.', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 8 MONTH)),
('addddddd-0011-0000-0000-000000000011', 'dddddddd-0004-0000-0000-000000000004', 'eeeeeeee-0002-0000-0000-000000000002', 85.00, 'Muy profesional, resuelve problemas complejos.', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 5 MONTH)),
('addddddd-0012-0000-0000-000000000012', 'dddddddd-0004-0000-0000-000000000004', 'eeeeeeee-0003-0000-0000-000000000003', 87.00, 'Gran desempeño, siempre puntual y eficiente.', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 2 MONTH));

-- ===== TÉCNICO 5 (Luis Fernández): Valoración media-baja (60) - Para Reporte 4 =====

INSERT INTO Assessments (Id, TechnicalId, DirectorId, Score, Comment, AssessmentDate) VALUES
('addddddd-0013-0000-0000-000000000013', 'dddddddd-0005-0000-0000-000000000005', 'eeeeeeee-0001-0000-0000-000000000001', 65.00, 'Rendimiento irregular, necesita supervisión.', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 6 MONTH)),
('addddddd-0014-0000-0000-000000000014', 'dddddddd-0005-0000-0000-000000000005', 'eeeeeeee-0002-0000-0000-000000000002', 55.00, 'Varios equipos fallaron después de su mantenimiento.', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 3 MONTH)),
('addddddd-0015-0000-0000-000000000015', 'dddddddd-0005-0000-0000-000000000005', 'eeeeeeee-0003-0000-0000-000000000003', 60.00, 'Cumple mínimamente, debe mejorar calidad del trabajo.', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 1 WEEK));

-- ---------------------------------------------------------------------
-- 2.11 BAJAS DE EQUIPOS (10 registros para Reportes 1, 4, 7)
-- ---------------------------------------------------------------------
-- DestinyTypeId: 1=Department, 2=Disposal, 3=Warehouse

-- ===== BAJAS PARA REPORTE 1 (Último año) =====

-- Baja 1: Equipo 1 - Fallo irreparable → Disposal (hace 6 meses)
INSERT INTO EquipmentDecommissions (Id, EquipmentId, TechnicalId, DepartmentId, DestinyTypeId, RecipientId, DecommissionDate, Reason) VALUES
('beeeeeee-0001-0000-0000-000000000001', '8bbbbbbb-0001-0000-0000-000000000001', 'dddddddd-0001-0000-0000-000000000001', '00000000-0000-0000-0000-000000000000', 2, NULL, DATE_SUB(UTC_TIMESTAMP(), INTERVAL 6 MONTH), 'fallo tecnico irreparable');

-- Baja 2: Equipo 2 - Obsolescencia → Warehouse (hace 2 meses)
INSERT INTO EquipmentDecommissions (Id, EquipmentId, TechnicalId, DepartmentId, DestinyTypeId, RecipientId, DecommissionDate, Reason) VALUES
('beeeeeee-0002-0000-0000-000000000002', '8bbbbbbb-0002-0000-0000-000000000002', 'dddddddd-0002-0000-0000-000000000002', '00000000-0000-0000-0000-000000000000', 3, '7aaaaaaa-0001-0000-0000-000000000001', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 2 MONTH), 'obsolescencia tecnológica');

-- Baja 3: Equipo 3 - Transferido a RRHH (hace 11 meses) → Department
INSERT INTO EquipmentDecommissions (Id, EquipmentId, TechnicalId, DepartmentId, DestinyTypeId, RecipientId, DecommissionDate, Reason) VALUES
('beeeeeee-0003-0000-0000-000000000003', '8bbbbbbb-0003-0000-0000-000000000003', 'dddddddd-0003-0000-0000-000000000003', 'bbbbbbbb-0003-0000-0000-000000000003', 1, '7aaaaaaa-0002-0000-0000-000000000002', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 11 MONTH), 'reubicación por cambio de área');

-- Baja 4: Equipo 4 - FUERA DEL RANGO (hace 14 meses) → NO aparece en R1
INSERT INTO EquipmentDecommissions (Id, EquipmentId, TechnicalId, DepartmentId, DestinyTypeId, RecipientId, DecommissionDate, Reason) VALUES
('beeeeeee-0004-0000-0000-000000000004', '8bbbbbbb-0004-0000-0000-000000000004', 'dddddddd-0004-0000-0000-000000000004', '00000000-0000-0000-0000-000000000000', 2, NULL, DATE_SUB(UTC_TIMESTAMP(), INTERVAL 14 MONTH), 'fallo técnico irreparable antiguo');

-- ===== BAJAS PARA REPORTE 4 (Fallo irreparable) =====

-- Baja 5: Equipo 5 - Fallo irreparable por técnico junior (Carlos) (hace 7 meses)
INSERT INTO EquipmentDecommissions (Id, EquipmentId, TechnicalId, DepartmentId, DestinyTypeId, RecipientId, DecommissionDate, Reason) VALUES
('beeeeeee-0005-0000-0000-000000000005', '8bbbbbbb-0005-0000-0000-000000000005', 'dddddddd-0003-0000-0000-000000000003', '00000000-0000-0000-0000-000000000000', 2, NULL, DATE_SUB(UTC_TIMESTAMP(), INTERVAL 7 MONTH), 'fallo tecnico irreparable');

-- Baja 6: Equipo 6 - Fallo irreparable (hace 6 meses)
INSERT INTO EquipmentDecommissions (Id, EquipmentId, TechnicalId, DepartmentId, DestinyTypeId, RecipientId, DecommissionDate, Reason) VALUES
('beeeeeee-0006-0000-0000-000000000006', '8bbbbbbb-0006-0000-0000-000000000006', 'dddddddd-0005-0000-0000-000000000005', '00000000-0000-0000-0000-000000000000', 2, NULL, DATE_SUB(UTC_TIMESTAMP(), INTERVAL 6 MONTH), 'fallo tecnico irreparable');

-- ===== BAJAS PARA REPORTE 7 (Equipo enviado a Desarrollo) =====

-- Baja 7: Equipo 13 - Enviado a Desarrollo (hace 4 meses)
INSERT INTO EquipmentDecommissions (Id, EquipmentId, TechnicalId, DepartmentId, DestinyTypeId, RecipientId, DecommissionDate, Reason) VALUES
('beeeeeee-0007-0000-0000-000000000007', '8bbbbbbb-0013-0000-0000-000000000013', 'dddddddd-0001-0000-0000-000000000001', 'bbbbbbbb-0001-0000-0000-000000000001', 1, '7aaaaaaa-0001-0000-0000-000000000001', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 4 MONTH), 'reasignación a departamento de desarrollo');

-- ===== BAJAS ADICIONALES (Casos variados) =====

-- Baja 8: A warehouse (hace 9 meses)
INSERT INTO EquipmentDecommissions (Id, EquipmentId, TechnicalId, DepartmentId, DestinyTypeId, RecipientId, DecommissionDate, Reason) VALUES
('beeeeeee-0008-0000-0000-000000000008', '8bbbbbbb-0002-0000-0000-000000000002', 'dddddddd-0002-0000-0000-000000000002', '00000000-0000-0000-0000-000000000000', 3, '7aaaaaaa-0001-0000-0000-000000000001', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 9 MONTH), 'almacenamiento temporal para revisión');

-- Baja 9: A disposal (hace 5 meses)
INSERT INTO EquipmentDecommissions (Id, EquipmentId, TechnicalId, DepartmentId, DestinyTypeId, RecipientId, DecommissionDate, Reason) VALUES
('beeeeeee-0009-0000-0000-000000000009', '8bbbbbbb-0001-0000-0000-000000000001', 'dddddddd-0004-0000-0000-000000000004', '00000000-0000-0000-0000-000000000000', 2, NULL, DATE_SUB(UTC_TIMESTAMP(), INTERVAL 5 MONTH), 'daño irreparable por agua');

-- Baja 10: A otro departamento (hace 3 meses)
INSERT INTO EquipmentDecommissions (Id, EquipmentId, TechnicalId, DepartmentId, DestinyTypeId, RecipientId, DecommissionDate, Reason) VALUES
('beeeeeee-0010-0000-0000-000000000010', '8bbbbbbb-0003-0000-0000-000000000003', 'dddddddd-0005-0000-0000-000000000005', 'bbbbbbbb-0004-0000-0000-000000000004', 1, '7aaaaaaa-0002-0000-0000-000000000002', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 3 MONTH), 'reasignación por reorganización');

-- ---------------------------------------------------------------------
-- 2.12 TRASLADOS (8 registros para Reportes 3 y 7)
-- ---------------------------------------------------------------------

-- ===== TRASLADOS PARA REPORTE 3 (Entre secciones vs dentro de sección) =====

-- Traslado 1: ENTRE SECCIONES (Tecnología → RRHH) - Equipo 11 → SÍ aparece en R3
INSERT INTO Transfers (Id, EquipmentId, SourceDepartmentId, TargetDepartmentId, ResponsibleId, RecipientId, TransferDate, CreatedAt) VALUES
('cfffffff-0001-0000-0000-000000000001', '8bbbbbbb-0011-0000-0000-000000000011', 'bbbbbbbb-0001-0000-0000-000000000001', 'bbbbbbbb-0003-0000-0000-000000000003', 'ffffffff-0001-0000-0000-000000000001', 'ffffffff-0003-0000-0000-000000000003', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 5 MONTH), DATE_SUB(UTC_TIMESTAMP(), INTERVAL 5 MONTH));

-- Traslado 2: DENTRO DE LA MISMA SECCIÓN (Desarrollo → Infraestructura, ambos en Tecnología) → NO aparece en R3
INSERT INTO Transfers (Id, EquipmentId, SourceDepartmentId, TargetDepartmentId, ResponsibleId, RecipientId, TransferDate, CreatedAt) VALUES
('cfffffff-0002-0000-0000-000000000002', '8bbbbbbb-0012-0000-0000-000000000012', 'bbbbbbbb-0001-0000-0000-000000000001', 'bbbbbbbb-0002-0000-0000-000000000002', 'ffffffff-0001-0000-0000-000000000001', 'ffffffff-0002-0000-0000-000000000002', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 3 MONTH), DATE_SUB(UTC_TIMESTAMP(), INTERVAL 3 MONTH));

-- Traslado 3: ENTRE SECCIONES (RRHH → Finanzas) → SÍ aparece en R3
INSERT INTO Transfers (Id, EquipmentId, SourceDepartmentId, TargetDepartmentId, ResponsibleId, RecipientId, TransferDate, CreatedAt) VALUES
('cfffffff-0003-0000-0000-000000000003', '8bbbbbbb-0009-0000-0000-000000000009', 'bbbbbbbb-0003-0000-0000-000000000003', 'bbbbbbbb-0005-0000-0000-000000000005', 'ffffffff-0003-0000-0000-000000000003', 'ffffffff-0005-0000-0000-000000000005', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 7 MONTH), DATE_SUB(UTC_TIMESTAMP(), INTERVAL 7 MONTH));

-- Traslado 4: DENTRO DE LA MISMA SECCIÓN (Gestión Talento → Capacitación, ambos en RRHH) → NO aparece en R3
INSERT INTO Transfers (Id, EquipmentId, SourceDepartmentId, TargetDepartmentId, ResponsibleId, RecipientId, TransferDate, CreatedAt) VALUES
('cfffffff-0004-0000-0000-000000000004', '8bbbbbbb-0010-0000-0000-000000000010', 'bbbbbbbb-0003-0000-0000-000000000003', 'bbbbbbbb-0004-0000-0000-000000000004', 'ffffffff-0003-0000-0000-000000000003', 'ffffffff-0004-0000-0000-000000000004', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 2 MONTH), DATE_SUB(UTC_TIMESTAMP(), INTERVAL 2 MONTH));

-- ===== TRASLADOS PARA REPORTE 7 (A Departamento Desarrollo) =====

-- Traslado 5: Equipo 14 → Desarrollo (hace 6 meses) → SÍ aparece en R7
INSERT INTO Transfers (Id, EquipmentId, SourceDepartmentId, TargetDepartmentId, ResponsibleId, RecipientId, TransferDate, CreatedAt) VALUES
('cfffffff-0005-0000-0000-000000000005', '8bbbbbbb-0014-0000-0000-000000000014', 'bbbbbbbb-0002-0000-0000-000000000002', 'bbbbbbbb-0001-0000-0000-000000000001', 'ffffffff-0002-0000-0000-000000000002', 'ffffffff-0001-0000-0000-000000000001', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 6 MONTH), DATE_SUB(UTC_TIMESTAMP(), INTERVAL 6 MONTH));

-- ===== TRASLADOS ADICIONALES (Variedad) =====

-- Traslado 6: ENTRE SECCIONES (Finanzas → Tecnología)
INSERT INTO Transfers (Id, EquipmentId, SourceDepartmentId, TargetDepartmentId, ResponsibleId, RecipientId, TransferDate, CreatedAt) VALUES
('cfffffff-0006-0000-0000-000000000006', '8bbbbbbb-0007-0000-0000-000000000007', 'bbbbbbbb-0005-0000-0000-000000000005', 'bbbbbbbb-0001-0000-0000-000000000001', 'ffffffff-0005-0000-0000-000000000005', 'ffffffff-0001-0000-0000-000000000001', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 8 MONTH), DATE_SUB(UTC_TIMESTAMP(), INTERVAL 8 MONTH));

-- Traslado 7: DENTRO DE LA MISMA SECCIÓN (Contabilidad → Auditoría, ambos en Finanzas)
INSERT INTO Transfers (Id, EquipmentId, SourceDepartmentId, TargetDepartmentId, ResponsibleId, RecipientId, TransferDate, CreatedAt) VALUES
('cfffffff-0007-0000-0000-000000000007', '8bbbbbbb-0008-0000-0000-000000000008', 'bbbbbbbb-0005-0000-0000-000000000005', 'bbbbbbbb-0006-0000-0000-000000000006', 'ffffffff-0005-0000-0000-000000000005', 'ffffffff-0005-0000-0000-000000000005', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 4 MONTH), DATE_SUB(UTC_TIMESTAMP(), INTERVAL 4 MONTH));

-- Traslado 8: ENTRE SECCIONES (Tecnología → Finanzas)
INSERT INTO Transfers (Id, EquipmentId, SourceDepartmentId, TargetDepartmentId, ResponsibleId, RecipientId, TransferDate, CreatedAt) VALUES
('cfffffff-0008-0000-0000-000000000008', '8bbbbbbb-0015-0000-0000-000000000015', 'bbbbbbbb-0001-0000-0000-000000000001', 'bbbbbbbb-0006-0000-0000-000000000006', 'ffffffff-0001-0000-0000-000000000001', 'ffffffff-0005-0000-0000-000000000005', DATE_SUB(UTC_TIMESTAMP(), INTERVAL 1 MONTH), DATE_SUB(UTC_TIMESTAMP(), INTERVAL 1 MONTH));
