// Alberto Hernández Sande
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAC6_UF2
{
    class Program
    {
        static void Main(string[] args)
        {
            // Menu de usuario
            int eleccion = 0;
            while (eleccion < 1 || eleccion > 4)
            {
                Console.Clear();
                Console.WriteLine("Bienvenido al juego de las parejas.");
                Console.WriteLine("1. Empezar una partida fácil");
                Console.WriteLine("2. Empezar una partida normal");
                Console.WriteLine("3. Empezar una partida difícil");
                Console.WriteLine("4. Salir");
                eleccion = int.Parse(Console.ReadLine());
            }
            Console.Clear();
            // Cargar elección
            switch (eleccion)
            {
                case 1: Dificultad(2);
                    break;
                case 2: Dificultad(4);
                    break;
                case 3: Dificultad(6);
                    break;
                default: break;
            }
        }

        static void Dificultad(int size)
        {
            // Se declaran dos matrices, una de solucion y otra de juego
            char[,] solucion;
            char[,] gameMatrix;
            Nuevo(size, out solucion, out gameMatrix);
            Play(solucion, gameMatrix, size);
        }

        static void Nuevo(int size, out char[,] solucion, out char[,] gameMatrix)
        {
            char[] matriz = new char[size * size];
            int unicode = 65; // Valor unicode para 'A'
            for (int i = 0; i < size * size; i++)
            {
                matriz[i] = (char)unicode;
                // Cada dos elementos, sumo uno al valor unicode para sacar la siguiente letra
                if (i % 2 != 0)
                {
                    unicode += 1;
                }
            }
            // Se barajan los valores del array
            // Lo mantengo unidimensional para simplificar Fisher-Yates
            matriz = Shuffle(matriz);
            // Preparación para rellenar la matriz solucion
            solucion = new char[size, size];
            // Preparación la matriz de juego
            gameMatrix = new char[size, size];
            int h = 0;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    solucion[i,j] = matriz[h++];
                    gameMatrix[i, j] = '?';
                }
            }
        }

        static char[] Shuffle(char[] matriz)
        {
            // Método Fisher-Yates
            int n = matriz.Length;
            Random rand = new Random();

            for (int i = 0; i < n; i++)
            {
                int r = i + (int)(rand.NextDouble() * (n - i));
                char t = matriz[r];
                matriz[r] = matriz[i];
                matriz[i] = t;
            }
            return matriz;
        }

        static void Play(char[,] solucion, char[,] gameMatrix, int size)
        {
            // Esta función muestra la matriz de juego y recoge los inputs
            int n = solucion.Length / 2;
            int solved = 0;
            int intentos = 0;
            string userInput1, userInput2;
            // Mientras el número de resueltos sea menor que el número de parejas
            // Mostrar matriz de juego y recoger inputs
            while (solved < n)
            {
                Console.Clear();
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        Console.Write(gameMatrix[i, j] + "\t");
                    }
                    Console.WriteLine("\n");
                }
                Console.WriteLine("Escriba la primera posición para emparejar");
                userInput1 = Console.ReadLine();
                Console.WriteLine("¿Con qué posición lo va a emparejar?");
                userInput2 = Console.ReadLine();
                // Se procede a comprobar y a realizar los cambios oportunos en la matriz
                Check(size, userInput1, userInput2, solucion, gameMatrix, ref intentos, ref solved);
            }
            Console.WriteLine("El número de intentos ha sido {0}", intentos);
            Console.ReadLine();
        }
        
        static void Check(int size, string userInput1, string userInput2, char[,] solucion, char[,] gameMatrix, ref int intentos, ref int solved)
        {
            Console.Clear();
            // Se definen posibles separadores y se guarda el valor a cada lado
            string[] separadores = { ".", "," };
            string[] position1 = userInput1.Split(separadores, System.StringSplitOptions.RemoveEmptyEntries);
            string[] position2 = userInput2.Split(separadores, System.StringSplitOptions.RemoveEmptyEntries);
            // Comprobar que las posiciones son diferentes
            if (position1[0] != position2[0] || position1[1] != position2[1])
            {
                // Comprobar que no están fuera de límites
                if (int.Parse(position1[0]) >=0 && int.Parse(position1[0]) < size && int.Parse(position1[1]) >=0 && int.Parse(position1[1]) < size && 
                    int.Parse(position2[0]) >=0 && int.Parse(position2[0]) < size && int.Parse(position2[1]) >=0 && int.Parse(position2[1]) < size)
                {
                    // Comprobar que ninguno ha sido revelado ya
                    if (gameMatrix[int.Parse(position1[0]), int.Parse(position1[1])] != solucion[int.Parse(position1[0]), int.Parse(position1[1])] &&
                        gameMatrix[int.Parse(position2[0]), int.Parse(position2[1])] != solucion[int.Parse(position2[0]), int.Parse(position2[1])])
                    {
                        // Desvelar los valores correspondientes
                        gameMatrix[int.Parse(position1[0]), int.Parse(position1[1])] = solucion[int.Parse(position1[0]), int.Parse(position1[1])];
                        gameMatrix[int.Parse(position2[0]), int.Parse(position2[1])] = solucion[int.Parse(position2[0]), int.Parse(position2[1])];
                        for (int i = 0; i < size; i++)
                        {
                            for (int j = 0; j < size; j++)
                            {
                                Console.Write(gameMatrix[i, j] + "\t");
                            }
                            Console.WriteLine("\n");
                        }
                        intentos++;
                        Console.ReadLine();
                        // Si los dos valores desvelados son iguales, el jugador ha acertado y se suma uno a la variable de resueltos
                        if (gameMatrix[int.Parse(position1[0]), int.Parse(position1[1])] == gameMatrix[int.Parse(position2[0]), int.Parse(position2[1])])
                        {
                            solved++;
                        }
                        else gameMatrix[int.Parse(position1[0]), int.Parse(position1[1])] = gameMatrix[int.Parse(position2[0]), int.Parse(position2[1])] = '?';
                    }
                    else
                    {
                        Console.WriteLine("Por favor, escribe posiciones nuevas");
                        Console.ReadLine();
                    }
 
                } else {
                    Console.WriteLine("Por favor, escribe posiciones válidas dentro de los límites\n(0 a {0})", size - 1);
                    Console.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("Por favor, escribe posiciones diferentes");
                Console.ReadLine();
            }
        }
    }
}
