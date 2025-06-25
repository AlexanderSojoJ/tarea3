using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace tarea4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Producto> productosTemporales = new List<Producto>();
            bool continuar = true;

            while (continuar)
            {
                // 1. Preguntar nombre del producto
                Console.Write("Escriba el nombre del producto: ");
                string nombre = Console.ReadLine();

                // Crear producto con fecha/hora actual
                Producto producto = new Producto
                {
                    Nombre = nombre,
                    FechaHora = DateTime.Now
                };

                // 2. Preguntar cantidad de números aleatorios
                int cantidad;
                do
                {
                    Console.Write("Escriba la cantidad de números aleatorios a generar: ");
                } while (!int.TryParse(Console.ReadLine(), out cantidad) || cantidad <= 0);

                bool puedenRepetir = true;

                // 3. Preguntar si se pueden repetir (solo si cantidad <= 100)
                if (cantidad <= 100)
                {
                    string respuesta;
                    do
                    {
                        Console.Write("¿Los números se pueden repetir? (s/n): ");
                        respuesta = Console.ReadLine().ToLower();
                    } while (respuesta != "s" && respuesta != "n");

                    puedenRepetir = (respuesta == "s");
                }

                // Generar números aleatorios
                GenerarNumeros(producto, cantidad, puedenRepetir);

                // Agregar producto a la lista temporal
                productosTemporales.Add(producto);

                // 4. Preguntar si desea agregar otro producto
                string continuar_respuesta;
                do
                {
                    Console.Write("¿Desea agregar otro producto? (s/n): ");
                    continuar_respuesta = Console.ReadLine().ToLower();
                } while (continuar_respuesta != "s" && continuar_respuesta != "n");

                continuar = (continuar_respuesta == "s");
            }

            // A. Guardar todos los productos en la base de datos
            GuardarEnBaseDatos(productosTemporales);

            // B. Mostrar todos los productos desde la base de datos
            MostrarProductosDesdeBaseDatos();

            // C. Finalizar programa
            Console.WriteLine("Programa finalizado. Presione la tecla enter para terminar.");
            Console.ReadLine();
        }

        static void GenerarNumeros(Producto producto, int cantidad, bool puedenRepetir)
        {
            Random random = new Random();
            List<int> numerosGenerados = new List<int>();

            for (int i = 1; i <= cantidad; i++)
            {
                int numero;

                if (puedenRepetir)
                {
                    // Pueden repetirse, generar cualquier número del 0-99
                    numero = random.Next(0, 100);
                }
                else
                {
                    // No pueden repetirse
                    do
                    {
                        numero = random.Next(0, 100);
                    } while (numerosGenerados.Contains(numero));
                }

                numerosGenerados.Add(numero);

                // Crear objeto Numero y agregarlo al producto
                Numero num = new Numero
                {
                    Orden = i,
                    Num = numero,
                    Producto = producto
                };

                producto.Numeros.Add(num);
            }
        }

        static void GuardarEnBaseDatos(List<Producto> productos)
        {
            using (var context = new TP3Context())
            {
                // Crear la base de datos si no existe
                context.Database.CreateIfNotExists();

                // Agregar todos los productos
                foreach (var producto in productos)
                {
                    context.Productos.Add(producto);
                }

                // Guardar cambios
                context.SaveChanges();
            }
        }

        static void MostrarProductosDesdeBaseDatos()
        {
            using (var context = new TP3Context())
            {
                // Obtener todos los productos ordenados por fecha/hora
                var productos = context.Productos
                    .Include("Numeros")
                    .OrderBy(p => p.FechaHora)
                    .ToList();

                int contadorProducto = 1;

                foreach (var producto in productos)
                {
                    Console.WriteLine($"{contadorProducto}. {producto.Nombre} - {producto.FechaHora:yyyy/MM/dd HH:mm}");

                    // Mostrar números ordenados por orden
                    var numerosOrdenados = producto.Numeros.OrderBy(n => n.Orden).ToList();

                    foreach (var numero in numerosOrdenados)
                    {
                        Console.WriteLine($"{numero.NumeroId}. [{numero.Orden}] {numero.Num}");
                    }

                    contadorProducto++;
                }
            }
        }
    }
}