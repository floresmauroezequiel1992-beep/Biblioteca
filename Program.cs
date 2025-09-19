using System;
using System.Collections.Generic;

// Clase Libro
public class Libro
{
    public string Titulo { get; set; }
    public string Autor { get; set; }
    public string ISBN { get; set; }
    public int AñoPublicacion { get; set; }

    public Libro(string titulo, string autor, string isbn, int añoPublicacion)
    {
        Titulo = titulo;
        Autor = autor;
        ISBN = isbn;
        AñoPublicacion = añoPublicacion;
    }

    public override string ToString()
    {
        return $"{Titulo} - {Autor} ({AñoPublicacion})";
    }
}

// Clase Lector
public class Lector
{
    public string Nombre { get; set; }
    public string Dni { get; set; }
    public List<Libro> LibrosPrestados { get; set; }
    private const int MAX_PRESTAMOS = 3;

    public Lector(string nombre, string dni)
    {
        Nombre = nombre;
        Dni = dni;
        LibrosPrestados = new List<Libro>();
    }

    public bool PuedeRetirarLibro()
    {
        return LibrosPrestados.Count < MAX_PRESTAMOS;
    }

    public void AgregarLibroPrestado(Libro libro)
    {
        if (PuedeRetirarLibro())
        {
            LibrosPrestados.Add(libro);
        }
    }

    public bool DevolverLibro(string titulo)
    {
        int i = 0;
        while (i < LibrosPrestados.Count)
        {
            if (LibrosPrestados[i].Titulo.Equals(titulo, StringComparison.OrdinalIgnoreCase))
            {
                LibrosPrestados.RemoveAt(i);
                return true;
            }
            i++;
        }
        return false;
    }

    public override string ToString()
    {
        return $"{Nombre} (DNI: {Dni}) - Préstamos activos: {LibrosPrestados.Count}/{MAX_PRESTAMOS}";
    }
}

// Clase principal Biblioteca
public class Biblioteca
{
    private List<Libro> libros;
    private List<Lector> lectores;

    // Constructor - instanciamos las listas para poder utilizarlas
    public Biblioteca()
    {
        libros = new List<Libro>();
        lectores = new List<Lector>();
    }

    // Método para buscar un libro por su nombre utilizando while y Count
    public Libro BuscarLibroPorTitulo(string titulo)
    {
        int i = 0;
        // Utilizamos Count para obtener la cantidad de elementos
        while (i < libros.Count)
        {
            // Accedemos a cada elemento a través de su índice con corchetes []
            if (libros[i].Titulo.Equals(titulo, StringComparison.OrdinalIgnoreCase))
            {
                // Una vez encontrado el libro, retornamos la referencia y salimos del ciclo
                return libros[i];
            }
            i++;
        }
        // Si no se encuentra, retornamos null
        return null;
    }

    // Método para buscar lector por DNI
    private Lector BuscarLectorPorDni(string dni)
    {
        int i = 0;
        while (i < lectores.Count)
        {
            if (lectores[i].Dni.Equals(dni, StringComparison.OrdinalIgnoreCase))
            {
                return lectores[i];
            }
            i++;
        }
        return null;
    }

    // Método altaLector - da de alta un lector si no está previamente registrado
    public string AltaLector(string nombre, string dni)
    {
        // Verificamos si el lector ya existe
        Lector lectorExistente = BuscarLectorPorDni(dni);

        if (lectorExistente != null)
        {
            return $"El lector con DNI {dni} ya está registrado.";
        }

        // Si no existe, lo agregamos a la lista
        Lector nuevoLector = new Lector(nombre, dni);
        lectores.Add(nuevoLector);

        return $"Lector {nombre} registrado exitosamente con DNI {dni}.";
    }

    // Método prestarLibro - maneja el préstamo de libros con todas las validaciones
    public string PrestarLibro(string titulo, string dni)
    {
        // Primero buscamos si existe el lector
        Lector lector = BuscarLectorPorDni(dni);
        if (lector == null)
        {
            return "LECTOR INEXISTENTE";
        }

        // Verificamos si el lector puede retirar más libros
        if (!lector.PuedeRetirarLibro())
        {
            return "TOPE DE PRESTAMO ALCAZADO";
        }

        // Buscamos el libro en la biblioteca
        Libro libro = BuscarLibroPorTitulo(titulo);
        if (libro == null)
        {
            return "LIBRO INEXISTENTE";
        }

        // Si todas las validaciones pasan, realizamos el préstamo
        // Removemos el libro de la biblioteca
        libros.Remove(libro);
        // Lo agregamos a la lista de libros prestados del lector
        lector.AgregarLibroPrestado(libro);

        return "PRESTAMO EXITOSO";
    }

    // Método para devolver un libro
    public string DevolverLibro(string titulo, string dni)
    {
        Lector lector = BuscarLectorPorDni(dni);
        if (lector == null)
        {
            return "LECTOR INEXISTENTE";
        }

        // Buscamos el libro en los préstamos del lector
        Libro libro = null;
        int i = 0;
        while (i < lector.LibrosPrestados.Count)
        {
            if (lector.LibrosPrestados[i].Titulo.Equals(titulo, StringComparison.OrdinalIgnoreCase))
            {
                libro = lector.LibrosPrestados[i];
                break;
            }
            i++;
        }

        if (libro == null)
        {
            return "EL LECTOR NO TIENE ESE LIBRO EN PRESTAMO";
        }

        // Devolvemos el libro a la biblioteca
        lector.DevolverLibro(titulo);
        libros.Add(libro);

        return "DEVOLUCION EXITOSA";
    }

    // Método para agregar libros a la biblioteca
    public void AgregarLibro(Libro libro)
    {
        libros.Add(libro);
    }

    // Método para mostrar todos los libros disponibles
    public void MostrarLibrosDisponibles()
    {
        Console.WriteLine("=== LIBROS DISPONIBLES EN BIBLIOTECA ===");
        if (libros.Count == 0)
        {
            Console.WriteLine("No hay libros disponibles.");
            return;
        }

        for (int i = 0; i < libros.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {libros[i]}");
        }
    }

    // Método para mostrar todos los lectores registrados
    public void MostrarLectoresRegistrados()
    {
        Console.WriteLine("=== LECTORES REGISTRADOS ===");
        if (lectores.Count == 0)
        {
            Console.WriteLine("No hay lectores registrados.");
            return;
        }

        for (int i = 0; i < lectores.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {lectores[i]}");

            // Mostrar libros prestados si los tiene
            if (lectores[i].LibrosPrestados.Count > 0)
            {
                Console.WriteLine("   Libros en préstamo:");
                for (int j = 0; j < lectores[i].LibrosPrestados.Count; j++)
                {
                    Console.WriteLine($"   - {lectores[i].LibrosPrestados[j]}");
                }
            }
        }
    }

    // Método para obtener estadísticas de la biblioteca
    public void MostrarEstadisticas()
    {
        Console.WriteLine("=== ESTADÍSTICAS DE LA BIBLIOTECA ===");
        Console.WriteLine($"Total de libros disponibles: {libros.Count}");
        Console.WriteLine($"Total de lectores registrados: {lectores.Count}");

        int totalPrestamos = 0;
        for (int i = 0; i < lectores.Count; i++)
        {
            totalPrestamos += lectores[i].LibrosPrestados.Count;
        }
        Console.WriteLine($"Total de préstamos activos: {totalPrestamos}");
    }
}

// Programa principal con ejemplos de uso
public class Program
{
    public static void Main()
    {
        // Creamos una instancia de la biblioteca
        Biblioteca biblioteca = new Biblioteca();

        Console.WriteLine("=== SISTEMA DE GESTIÓN DE BIBLIOTECA ===\n");

        // Agregamos algunos libros a la biblioteca
        biblioteca.AgregarLibro(new Libro("Cien años de soledad", "Gabriel García Márquez", "978-84-376-0494-7", 1967));
        biblioteca.AgregarLibro(new Libro("1984", "George Orwell", "978-84-9759-252-5", 1949));
        biblioteca.AgregarLibro(new Libro("El Quijote", "Miguel de Cervantes", "978-84-376-0495-4", 1605));
        biblioteca.AgregarLibro(new Libro("Rayuela", "Julio Cortázar", "978-84-376-0496-1", 1963));
        biblioteca.AgregarLibro(new Libro("Ficciones", "Jorge Luis Borges", "978-84-376-0497-8", 1944));

        // Mostramos los libros disponibles
        biblioteca.MostrarLibrosDisponibles();
        Console.WriteLine();

        // Registramos algunos lectores
        Console.WriteLine("=== REGISTRO DE LECTORES ===");
        Console.WriteLine(biblioteca.AltaLector("Juan Pérez", "12345678"));
        Console.WriteLine(biblioteca.AltaLector("María González", "87654321"));
        Console.WriteLine(biblioteca.AltaLector("Carlos López", "11223344"));

        // Intentamos registrar un lector duplicado 
        Console.WriteLine(biblioteca.AltaLector("Juan Pérez", "12345678"));
        Console.WriteLine();

        // Mostramos los lectores registrados
        biblioteca.MostrarLectoresRegistrados();
        Console.WriteLine();

        // Realizamos algunos préstamos
        Console.WriteLine("=== PRUEBAS DE PRÉSTAMOS ===");

        // Préstamos exitosos
        Console.WriteLine($"Préstamo 1: {biblioteca.PrestarLibro("1984", "12345678")}");
        Console.WriteLine($"Préstamo 2: {biblioteca.PrestarLibro("El Quijote", "12345678")}");
        Console.WriteLine($"Préstamo 3: {biblioteca.PrestarLibro("Rayuela", "87654321")}");

        // Intento de préstamo de libro inexistente
        Console.WriteLine($"Préstamo 4: {biblioteca.PrestarLibro("Libro Inexistente", "12345678")}");

        // Intento de préstamo con lector inexistente
        Console.WriteLine($"Préstamo 5: {biblioteca.PrestarLibro("Ficciones", "99999999")}");

        // Llenamos el cupo máximo de un lector
        Console.WriteLine($"Préstamo 6: {biblioteca.PrestarLibro("Ficciones", "12345678")}");
        Console.WriteLine($"Préstamo 7: {biblioteca.PrestarLibro("Cien años de soledad", "12345678")}"); // Debería fallar por tope
        Console.WriteLine();

        // Mostramos el estado actual
        Console.WriteLine("=== ESTADO DESPUÉS DE LOS PRÉSTAMOS ===");
        biblioteca.MostrarLectoresRegistrados();
        Console.WriteLine();
        biblioteca.MostrarLibrosDisponibles();
        Console.WriteLine();
        biblioteca.MostrarEstadisticas();
        Console.WriteLine();

        // Realizamos una devolución
        Console.WriteLine("=== PRUEBA DE DEVOLUCIÓN ===");
        Console.WriteLine($"Devolución: {biblioteca.DevolverLibro("1984", "12345678")}");
        Console.WriteLine();

        // Mostramos el estado final
        Console.WriteLine("=== ESTADO FINAL ===");
        biblioteca.MostrarLectoresRegistrados();
        Console.WriteLine();
        biblioteca.MostrarLibrosDisponibles();
        Console.WriteLine();
        biblioteca.MostrarEstadisticas();

        Console.WriteLine("\nPresione cualquier tecla para salir...");
        Console.ReadKey();
    }
}