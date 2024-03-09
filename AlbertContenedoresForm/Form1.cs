using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlbertContenedoresForm
{
    public partial class Form1 : Form
    {
        public abstract class Contenedor
        {
            public int Peso { get; set; }

            public Contenedor(int peso)
            {
                Peso = peso;
            }
        }

        public class ContenedorNormal : Contenedor
        {
            public ContenedorNormal(int peso) : base(peso)
            {
            }
        }

        public class ContenedorSeguridad : Contenedor
        {
            public bool Alarma { get; set; }

            public ContenedorSeguridad(int peso) : base(peso)
            {
                
            } 
        }

        public class ContenedorTemperatura : Contenedor
        {
            public int Temperatura { get; set; }

            public ContenedorTemperatura(int peso) : base(peso)
            {
            }
        }

        public interface IShow
        {
            void MostrarContenedores();
        }

        public class PilaContenedores
        {
            public Stack<Contenedor> Contenedores { get; set; } = new Stack<Contenedor>();
            public string Nombre { get; set; } = string.Empty;
            public int PesoPila { get; set; }

            public PilaContenedores(string nombre, int pesoPila)
            {
                Nombre = nombre;
                PesoPila = pesoPila;
            }

            public void AgregarContenedor(Contenedor contenedor)
            {
                Contenedores.Push(contenedor);
                PesoPila += contenedor.Peso;
            }
        }

        public class ColaCamiones
        {
            public Queue<Camion> Camiones { get; set; } = new Queue<Camion>();
            public string Nombre { get; set; } = string.Empty;

            public ColaCamiones(string nombre)
            {
                Nombre = nombre;
            }
        }

        public class Barco
        {
            public PilaContenedores[] PilasContenedores { get; set; }

            public Barco(PilaContenedores[] pilasContenedores)
            {
                PilasContenedores = pilasContenedores;
            }
        }

        public class Camion : IShow
        {
            public List<Contenedor> Contenedores { get; set; }
            public int NumMaximoContenedores { get; set; }
            public string Nombre { get; set; } = string.Empty;

            public Camion(List<Contenedor> contenedores, int numMaximoContenedores, string nombre)
            {
                Nombre = nombre;
                NumMaximoContenedores = numMaximoContenedores;
                Contenedores = contenedores;
            }

            public override string ToString()
            {
                return $"Nombre: {Nombre}";
            }

            public void MostrarContenedores()
            {
                foreach (var contenedor in Contenedores)
                {
                    if (contenedor is ContenedorNormal)
                    {
                        Console.WriteLine($"    Tipo: ContenedorNormal, Peso: {contenedor.Peso}");
                    }
                    else if (contenedor is ContenedorSeguridad)
                    {
                        Console.WriteLine($"    Tipo: ContenedorSeguridad, Alarma: {((ContenedorSeguridad)contenedor).Alarma}, Peso: {contenedor.Peso}");
                    }
                    else if (contenedor is ContenedorTemperatura)
                    {
                        Console.WriteLine($"    Tipo: ContenedorRefrigerado, Temperatura: {((ContenedorTemperatura)contenedor).Temperatura}, Peso: {contenedor.Peso}");
                    }
                }
            }

        }

        Barco barco = new Barco(new PilaContenedores[5]);
        ColaCamiones colaCamiones = new ColaCamiones("Cola de camiones");

        public void Initialize()
        {
            for (int i = 0; i < 5; i++)
            {
                barco.PilasContenedores[i] = new PilaContenedores($"Pila {i + 1}", 0);

            }

            barco.PilasContenedores[0].AgregarContenedor(new ContenedorNormal(100));
            barco.PilasContenedores[0].AgregarContenedor(new ContenedorNormal(130));
            barco.PilasContenedores[1].AgregarContenedor(new ContenedorTemperatura(200));
            barco.PilasContenedores[2].AgregarContenedor(new ContenedorTemperatura(50));
            barco.PilasContenedores[2].AgregarContenedor(new ContenedorSeguridad(100));
            barco.PilasContenedores[2].AgregarContenedor(new ContenedorNormal(400));
            barco.PilasContenedores[3].AgregarContenedor(new ContenedorSeguridad(100));
            barco.PilasContenedores[4].AgregarContenedor(new ContenedorSeguridad(100));

            for (int i = 0; i < 6; i++)
            {
                colaCamiones.Camiones.Enqueue(new Camion(new List<Contenedor>(), 2, $"Camion {i + 1}"));
            }


            colaCamiones.Camiones.ElementAt(4).Contenedores.Add(new ContenedorNormal(100));
            colaCamiones.Camiones.ElementAt(5).Contenedores.Add(new ContenedorTemperatura(150));

            
        }

        public string MostrarContenedores(Barco barco)
        {
            StringBuilder sb = new StringBuilder();

            if (barco?.PilasContenedores != null)
            {
                for (int i = 0; i < barco.PilasContenedores.Length; i++)
                {
                    if (barco.PilasContenedores[i]?.PesoPila != null)
                    {
                        sb.AppendLine($"Pila A{i}: Peso Pila: {barco.PilasContenedores[i].PesoPila}");
                        if (barco.PilasContenedores[i]?.Contenedores != null)
                        {
                            foreach (var contenedor in barco.PilasContenedores[i].Contenedores)
                            {
                                if (contenedor is ContenedorNormal)
                                {
                                    sb.AppendLine($"     Tipo: ContenedorNormal Peso: {contenedor.Peso}");
                                }
                                else if (contenedor is ContenedorSeguridad)
                                {
                                    sb.AppendLine($"     Tipo: ContenedorSeguridad Alarma: {((ContenedorSeguridad)contenedor).Alarma} Peso: {contenedor.Peso}");
                                }
                                else if (contenedor is ContenedorTemperatura)
                                {
                                    sb.AppendLine($"     Tipo: ContenedorRefrigerado Temperatura: {((ContenedorTemperatura)contenedor).Temperatura} Peso: {contenedor.Peso}");
                                }
                            }
                        }
                    }
                }
            }
            return sb.ToString();
        }



        public string MostrarPesoContenedores(Barco barco)
        {
            StringBuilder sb = new StringBuilder();
            if (barco?.PilasContenedores != null)
            {
                for (int i = 0; i < barco.PilasContenedores.Length; i++)
                {
                    if (barco.PilasContenedores[i]?.PesoPila != null)
                    {
                        sb.AppendLine($"Pila A{i}: Peso Pila: {barco.PilasContenedores[i].PesoPila}");
                    }
                }
            }
            return sb.ToString();
        }

        public string ActivarSeguridad(Barco barco)
        {
            if (barco?.PilasContenedores != null)
            {
                for (int i = 0; i < barco.PilasContenedores.Length; i++)
                {
                    foreach (var contenedor in barco.PilasContenedores[i].Contenedores)
                    {
                        if (contenedor is ContenedorSeguridad)
                        {
                            ((ContenedorSeguridad)contenedor).Alarma = true;
                        }
                    }
                }
            }
            return "Seguridad activada.";
        }

        public string ActivarTemperatura(Barco barco)
        {
            if (barco?.PilasContenedores != null)
            {
                for (int i = 0; i < barco.PilasContenedores.Length; i++)
                {
                    foreach (var contenedor in barco.PilasContenedores[i].Contenedores)
                    {
                        if (contenedor is ContenedorTemperatura)
                        {
                            ((ContenedorTemperatura)contenedor).Temperatura = 7;
                        }
                    }
                }
            }
            return "Temperatura activada a 7 grados.";
        }

        public string MostrarContenedoresOrdenadosPeso(Barco barco)
        {
            StringBuilder sb = new StringBuilder();
            if (barco?.PilasContenedores != null)
            {
                var pilasOrdenadas = barco.PilasContenedores.OrderBy(pila => pila.PesoPila).ToArray(); // Ordenar las pilas por peso
                for (int i = 0; i < pilasOrdenadas.Length; i++)
                {
                    if (pilasOrdenadas[i]?.PesoPila != null)
                    {
                        sb.AppendLine($"Pila A{i}: Peso Pila: {pilasOrdenadas[i].PesoPila}");
                        foreach (var contenedor in pilasOrdenadas[i].Contenedores)
                        {
                            if (contenedor is ContenedorNormal)
                            {
                                sb.AppendLine($"     Tipo: ContenedorNormal Peso: {contenedor.Peso}");
                            }
                            else if (contenedor is ContenedorSeguridad)
                            {
                                sb.AppendLine($"     Tipo: ContenedorSeguridad Alarma: {((ContenedorSeguridad)contenedor).Alarma} Peso: {contenedor.Peso}");
                            }
                            else if (contenedor is ContenedorTemperatura)
                            {
                                sb.AppendLine($"     Tipo: ContenedorRefrigerado Temperatura: {((ContenedorTemperatura)contenedor).Temperatura} Peso: {contenedor.Peso}");
                            }
                        }
                    }
                }
            }
            return sb.ToString();
        }


        public string MostrarColaCamiones(ColaCamiones colaCamiones)
        {
            StringBuilder sb = new StringBuilder();
            int i = 1;
            foreach (var camion in colaCamiones.Camiones)
            {
                sb.AppendLine($"Camion {i++}: {camion.Nombre}");
            }
            return sb.ToString();
        }


        public string MostrarPilaContenedoresMaxima(Barco barco)
        {
            StringBuilder sb = new StringBuilder();
            var pilaMaxima = barco.PilasContenedores.OrderByDescending(pila => pila.PesoPila).First(); // Pila con el peso máximo
            int indice = Array.IndexOf(barco.PilasContenedores, pilaMaxima);
            sb.AppendLine($"Pila de Contenedores con Peso Máximo:");
            sb.AppendLine($"   Pila: A{indice}     Peso:  {pilaMaxima.PesoPila}:");
            foreach (var contenedor in pilaMaxima.Contenedores)
            {
                if (contenedor is ContenedorNormal)
                {
                    sb.AppendLine($"     Tipo: ContenedorNormal Peso: {contenedor.Peso}");
                }
                else if (contenedor is ContenedorSeguridad)
                {
                    sb.AppendLine($"     Tipo: ContenedorSeguridad Alarma: {((ContenedorSeguridad)contenedor).Alarma} Peso: {contenedor.Peso}");
                }
                else if (contenedor is ContenedorTemperatura)
                {
                    sb.AppendLine($"     Tipo: ContenedorRefrigerado Temperatura: {((ContenedorTemperatura)contenedor).Temperatura} Peso: {contenedor.Peso}");
                }
            }
            return sb.ToString();
        }


        public List<string> DescargarContenedores(Barco barco, ColaCamiones colaCamiones)
        {
            List<string> resultados = new List<string>
            {
                MostrarEstadoCamiones(colaCamiones),
                MostrarContenedores(barco),
                DescargarContenedor(barco, colaCamiones),
                MostrarEstadoCamiones(colaCamiones),
                DescargarContenedor(barco, colaCamiones),
                MostrarEstadoCamiones(colaCamiones),
                DescargarContenedor(barco, colaCamiones),
                MostrarEstadoCamiones(colaCamiones),
                MostrarContenedores(barco)
            };
            return resultados;
        }

        public string DescargarContenedor(Barco barco, ColaCamiones colaCamiones)
        {
            StringBuilder sb = new StringBuilder();
            if (barco.PilasContenedores.Any(pila => pila.Contenedores.Count > 0))
            {
                var pilaConContenedores = barco.PilasContenedores.First(pila => pila.Contenedores.Count > 0);
                var contenedor = pilaConContenedores.Contenedores.Pop();
                if (colaCamiones.Camiones.Count > 0)
                {
                    var camion = colaCamiones.Camiones.Peek();
                    if (camion.Contenedores.Count < camion.NumMaximoContenedores)
                    {
                        camion.Contenedores.Add(contenedor);
                        sb.AppendLine("Contenedor descargado del barco al camión.");
                    }
                    else
                    {
                        sb.AppendLine("El camión ya está lleno. No se puede descargar el contenedor.");
                    }
                }
                else
                {
                    sb.AppendLine("No hay camiones en la cola. No se puede descargar el contenedor.");
                }
            }
            else
            {
                sb.AppendLine("No hay contenedores en el barco para descargar.");
            }
            return sb.ToString();
        }

        public string MostrarEstadoCamiones(ColaCamiones colaCamiones)
        {
            StringBuilder sb = new StringBuilder();
            int i = 1;
            foreach (var camion in colaCamiones.Camiones)
            {
                sb.AppendLine($"Camion {i} Peso Contenedores :{camion.Contenedores.Sum(contenedor => contenedor.Peso)}");
                foreach (var contenedor in camion.Contenedores)
                {
                    if (contenedor is ContenedorNormal)
                    {
                        sb.AppendLine($"    Tipo: ContenedorNormal, Peso: {contenedor.Peso}");
                    }
                    else if (contenedor is ContenedorSeguridad)
                    {
                        sb.AppendLine($"    Tipo: ContenedorSeguridad, Alarma: {((ContenedorSeguridad)contenedor).Alarma}, Peso: {contenedor.Peso}");
                    }
                    else if (contenedor is ContenedorTemperatura)
                    {
                        sb.AppendLine($"    Tipo: ContenedorRefrigerado, Temperatura: {((ContenedorTemperatura)contenedor).Temperatura}, Peso: {contenedor.Peso}");
                    }
                }
                i++;
            }
            return sb.ToString();
        }

        public string MostrarEstadoInicialCamiones(ColaCamiones colaCamiones)
        {
            StringBuilder sb = new StringBuilder();
            int i = 1;
            foreach (var camion in colaCamiones.Camiones)
            {
                int pesoTotalContenedores = camion.Contenedores.Sum(contenedor => contenedor.Peso);
                sb.AppendLine($"Camion {i} Peso Contenedores :{pesoTotalContenedores}");
                foreach (var contenedor in camion.Contenedores)
                {
                    if (contenedor is ContenedorNormal)
                    {
                        sb.AppendLine($"    Tipo: ContenedorNormal, Peso: {contenedor.Peso}");
                    }
                    else if (contenedor is ContenedorSeguridad)
                    {
                        sb.AppendLine($"    Tipo: ContenedorSeguridad, Alarma: {((ContenedorSeguridad)contenedor).Alarma}, Peso: {contenedor.Peso}");
                    }
                    else if (contenedor is ContenedorTemperatura)
                    {
                        sb.AppendLine($"    Tipo: ContenedorRefrigerado, Temperatura: {((ContenedorTemperatura)contenedor).Temperatura}, Peso: {contenedor.Peso}");
                    }
                }
                i++;
            }
            return sb.ToString();
        }

        public string QuitarCamionCola(ColaCamiones colaCamiones)
        {
            var camionQuitado = colaCamiones.Camiones.Dequeue();
            return $"Se quitó el {camionQuitado.Nombre} de la cola.";
        }

        public Form1()
        {
            InitializeComponent();
            Initialize();

        }


        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                switch (button.Name)
                {
                    case "button1":
                        textBoxResultados.Text = MostrarContenedores(barco);
                        break;
                    case "button2":
                        textBoxResultados.Text = MostrarPesoContenedores(barco);
                        break;
                    case "button3":
                        textBoxResultados.Text = ActivarSeguridad(barco);
                        break;
                    case "button4":
                        textBoxResultados.Text = ActivarTemperatura(barco);
                        break;
                    case "button5":
                        textBoxResultados.Text = MostrarContenedoresOrdenadosPeso(barco);
                        break;
                    case "button6":
                        textBoxResultados.Text = MostrarColaCamiones(colaCamiones);
                        break;
                    case "button7":
                        textBoxResultados.Text = MostrarPilaContenedoresMaxima(barco);
                        break;
                    case "button8":
                        textBoxResultados.Text = string.Join(Environment.NewLine, DescargarContenedores(barco, colaCamiones));
                        break;
                    case "button9":
                        textBoxResultados.Text = MostrarEstadoInicialCamiones(colaCamiones);
                        break;
                    case "button10":
                        textBoxResultados.Text = QuitarCamionCola(colaCamiones);
                        break;
                    default:
                        textBoxResultados.Text = "Botón no reconocido.";
                        break;
                }
            }
        }

    }
}
