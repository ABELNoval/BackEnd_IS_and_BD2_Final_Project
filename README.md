# 🚀 Gestión de Bajas Técnicas - Backend

Este repositorio contiene el **backend** del sistema de **Gestión de Bajas Técnicas**, desarrollado como parte del proyecto académico de **Bases de Datos II** e **Ingeniería de Software** (UH, curso 2025-2026).

El sistema responde a la necesidad de una **empresa de infocomunicaciones** de gestionar de manera eficiente el inventario de equipos, sus mantenimientos, bajas y traslados, eliminando el manejo manual que genera riesgo de pérdida de información.

---

## 📌 Descripción del proyecto

El **Sistema de Gestión de Bajas Técnicas** permite automatizar y optimizar la gestión de:

- 📦 **Inventario de equipos**: identificación, tipo, estado, ubicación y fecha de adquisición.  
- 🔧 **Mantenimientos**: historial de mantenimientos con fecha, tipo, costo y técnico responsable.  
- ❌ **Bajas técnicas**: registro de causa (obsolescencia, fallo irreparable, etc.), fecha, destino final y receptor.  
- 🔄 **Traslados**: control de equipos enviados entre unidades, incluyendo fecha, origen, destino y responsables.  
- 👷 **Técnicos**: información personal, especialidad, años de experiencia, rendimiento y su historial de intervenciones.  
- 📊 **Reportes**: consultas avanzadas con tablas y gráficos, incluyendo exportación a PDF.  

El sistema define distintos roles de usuario:
- **Director del centro** → control total y generación de reportes.  
- **Responsables de secciones** → solicitar traslados y revisar inventarios de su área.  
- **Técnicos** → registrar mantenimientos y bajas.  
- **Receptores** → confirmar recepción y destino de equipos.  

---

## 📡 Funcionalidades clave (a nivel backend)

1. Listado de equipos dados de baja en el último año, con causa, destino y receptor.  
2. Historial de mantenimientos de un equipo, con técnicos responsables.  
3. Registro de traslados entre secciones (fechas, origen, destino, responsables).  
4. Reporte de correlación entre rendimiento de técnicos y longevidad de equipos, incluyendo costos de mantenimiento.  
5. Detección de equipos con más de tres mantenimientos en el último año (reemplazo obligatorio).  
6. Comparación de técnicos para determinar bonificaciones o penalizaciones según rendimiento e intervenciones.  
7. Reporte de equipos enviados a un departamento específico con responsables de envío y recepción.  
8. Exportación de resultados a **PDF** y ordenamiento de columnas dinámico.  

---

## 📌 Tecnologías utilizadas

- [ASP.NET Core 8](https://learn.microsoft.com/aspnet/core) – Framework backend  
- [Entity Framework Core](https://learn.microsoft.com/ef/core/) – ORM para acceso a datos  
- [SQL Server / SQLite] – Base de datos  
- [Swagger](https://swagger.io/) – Documentación interactiva de la API  
- [xUnit](https://xunit.net/) – Pruebas unitarias  

---

## ⚙️ Instalación y ejecución

1. Clonar el repositorio:
   ```bash
   git clone https://github.com/ABELNoval/BackEnd_IS_and_BD2_Final_Project.git
   cd BackEnd_IS_and_BD2_Final_Project
