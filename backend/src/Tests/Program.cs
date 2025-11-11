using Tests;

Console.WriteLine("===== 🧪 TEST DE MANTENIMIENTO (INMEMORY) =====");

bool running = true;
while (running)
{
    Console.WriteLine("\nSelecciona una acción:");
    Console.WriteLine("1️⃣ Crear entidades");
    Console.WriteLine("2️⃣ Actualizar entidades");
    Console.WriteLine("3️⃣ Eliminar entidades");
    Console.WriteLine("4️⃣ Salir");

    Console.Write("👉 Opción: ");
    var input = Console.ReadLine();

    switch (input)
    {
        case "1":
            await MaintenanceTestRunner.CreateEntitiesAsync();
            break;
        case "2":
            await MaintenanceTestRunner.UpdateEntitiesAsync();
            break;
        case "3":
            await MaintenanceTestRunner.DeleteEntitiesAsync();
            break;
        case "4":
            running = false;
            Console.WriteLine("👋 Saliendo...");
            break;
        default:
            Console.WriteLine("❌ Opción no válida.");
            break;
    }
}
