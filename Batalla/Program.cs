using System;

namespace PokemonBattle
{
    // Interfaz para definir habilidades de combate
    public interface IHabilidad
    {
        int Atacar();
        int Defender();
    }

    // Clase abstracta que implementa la interfaz IHabilidad
    public abstract class PokemonBase : IHabilidad
    {
        private string nombre;
        private string tipo;
        private int[] ataques;
        private int defensa;
        private Random random;

        public PokemonBase(string nombre, string tipo, int[] ataques, int defensa)
        {
            this.nombre = nombre;
            this.tipo = tipo;
            this.ataques = ataques;
            this.defensa = defensa;
            this.random = new Random();
        }

        public string ObtenerNombre() => nombre;

        // Método privado para obtener un ataque aleatorio
        protected int GetAtaqueAleatorio()
        {
            int ataqueSeleccionado = ataques[random.Next(ataques.Length)];
            int factor = random.Next(0, 2); // Genera 0 o 1
            return ataqueSeleccionado * factor;
        }

        // Método privado para calcular la defensa
        protected int GetDefensa()
        {
            double factor = random.NextDouble() < 0.5 ? 0.5 : 1;
            return (int)(defensa * factor);
        }

        public abstract int Atacar();
        public abstract int Defender();
    }

    // Clase Pokemon que hereda de la clase abstracta PokemonBase
    public class Pokemon : PokemonBase
    {
        public Pokemon(string nombre, string tipo, int[] ataques, int defensa)
            : base(nombre, tipo, ataques, defensa) { }

        // Implementación de la habilidad de atacar
        public override int Atacar()
        {
            int daño = GetAtaqueAleatorio();
            Console.WriteLine($"{ObtenerNombre()} ataca con un daño de {daño}.");
            return daño;
        }

        // Implementación de la habilidad de defender
        public override int Defender()
        {
            int resistencia = GetDefensa();
            Console.WriteLine($"{ObtenerNombre()} se defiende con una resistencia de {resistencia}.");
            return resistencia;
        }
    }

    // Clase para el combate entre dos Pokémon
    public class Combate
    {
        private Pokemon pokemon1;
        private Pokemon pokemon2;

        public Combate(Pokemon pokemon1, Pokemon pokemon2)
        {
            this.pokemon1 = pokemon1;
            this.pokemon2 = pokemon2;
        }

        public void IniciarCombate()
        {
            int puntajePokemon1 = 0;
            int puntajePokemon2 = 0;

            for (int turno = 1; turno <= 3; turno++)
            {
                Console.WriteLine($"\n--- Turno {turno} ---");

                // Pokémon 1 ataca y Pokémon 2 se defiende
                int daño1 = pokemon1.Atacar();
                int defensa2 = pokemon2.Defender();
                puntajePokemon1 += Math.Max(0, daño1 - defensa2);

                // Pokémon 2 ataca y Pokémon 1 se defiende
                int daño2 = pokemon2.Atacar();
                int defensa1 = pokemon1.Defender();
                puntajePokemon2 += Math.Max(0, daño2 - defensa1);
            }

            Console.WriteLine("\n--- Resultado Final ---");
            Console.WriteLine($"{pokemon1.ObtenerNombre()} tiene un puntaje de: {puntajePokemon1}");
            Console.WriteLine($"{pokemon2.ObtenerNombre()} tiene un puntaje de: {puntajePokemon2}");

            if (puntajePokemon1 > puntajePokemon2)
            {
                Console.WriteLine($"¡{pokemon1.ObtenerNombre()} gana el combate!");
            }
            else if (puntajePokemon2 > puntajePokemon1)
            {
                Console.WriteLine($"¡{pokemon2.ObtenerNombre()} gana el combate!");
            }
            else
            {
                Console.WriteLine("El combate terminó en empate.");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Crear Pokémon 1
            Console.WriteLine("Ingrese los datos para el Pokémon 1:");
            Pokemon pokemon1 = CrearPokemon();

            // Crear Pokémon 2
            Console.WriteLine("\nIngrese los datos para el Pokémon 2:");
            Pokemon pokemon2 = CrearPokemon();

            // Iniciar combate
            Combate combate = new Combate(pokemon1, pokemon2);
            combate.IniciarCombate();
        }

        static Pokemon CrearPokemon()
        {
            Console.Write("Nombre: ");
            string nombre = Console.ReadLine();

            Console.Write("Tipo: ");
            string tipo = Console.ReadLine();

            int[] ataques = new int[3];
            for (int i = 0; i < 3; i++)
            {
                Console.Write($"Ataque {i + 1} (0-40): ");
                ataques[i] = int.Parse(Console.ReadLine());
                while (ataques[i] < 0 || ataques[i] > 40)
                {
                    Console.Write("Valor inválido. Ingrese un número entre 0 y 40: ");
                    ataques[i] = int.Parse(Console.ReadLine());
                }
            }

            Console.Write("Defensa (10-35): ");
            int defensa = int.Parse(Console.ReadLine());
            while (defensa < 10 || defensa > 35)
            {
                Console.Write("Valor inválido. Ingrese un número entre 10 y 35: ");
                defensa = int.Parse(Console.ReadLine());
            }

            return new Pokemon(nombre, tipo, ataques, defensa);
        }
    }
}