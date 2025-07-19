using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaHospitalRegistroCompleto
{
    // Clase base que representa a cualquier usuario del hospital
    class UsuarioHospital
    {
        public string Nombre { get; set; }   // Nombre del usuario
        public string DNI { get; set; }      // Documento de identidad

        public UsuarioHospital(string nombre, string dni)
        {
            Nombre = nombre;
            DNI = dni;
        }

        public override string ToString() { return Nombre + " | DNI: " + DNI; }
    }

    // Clase Paciente que hereda de UsuarioHospital
    class Paciente : UsuarioHospital
    {
        public string HistoriaMedica { get; set; }   // Número de historia médica

        public Paciente(string nombre, string historia, string dni)
            : base(nombre, dni)
        {
            HistoriaMedica = historia;
        }

        public override string ToString()
        {
            return "Paciente: " + Nombre + " | Historia: " + HistoriaMedica + " | DNI: " + DNI;
        }
    }

    // Clase para representar una especialidad médica
    class Especialidad
    {
        public string Nombre { get; private set; }

        public Especialidad(string nombre)
        {
            Nombre = nombre;
        }

        public override string ToString()
        {
            return Nombre;
        }
    }

    // Clase Doctor que hereda de UsuarioHospital
    class Doctor : UsuarioHospital
    {
        public string CodigoMedico { get; private set; }           // Código del colegio médico
        public Especialidad Especialidad { get; private set; }      // Especialidad médica
        public List<DateTime> HorariosDisponibles { get; private set; } // Lista de horarios para citas

        public Doctor(string nombre, string dni, string codigoMedico, Especialidad especialidad, List<DateTime> horarios)
            : base(nombre, dni)
        {
            CodigoMedico = codigoMedico;
            Especialidad = especialidad;
            HorariosDisponibles = horarios;
        }

        public override string ToString()
        {
            return "Dr. " + Nombre + " | " + Especialidad + " | Cód: " + CodigoMedico;
        }
    }

    // Clase Enfermera que hereda de UsuarioHospital
    class Enfermera : UsuarioHospital
    {
        public string CodigoEnfermera { get; private set; } // Código único de enfermera

        public Enfermera(string nombre, string dni, string codigo)
            : base(nombre, dni)
        {
            CodigoEnfermera = codigo;
        }

        public override string ToString()
        {
            return "Enfermera: " + Nombre + " | Código: " + CodigoEnfermera;
        }
    }

    // Clase para representar una cita médica
    class CitaMedica
    {
        public int Id { get; private set; }          // ID único para la cita
        public Paciente Paciente { get; private set; } // Paciente relacionado
        public Doctor Doctor { get; private set; }     // Doctor relacionado
        public DateTime Horario { get; private set; }  // Fecha y hora de la cita
        public string Ticket { get; private set; }     // Código único (ticket) para la cita

        public Especialidad Especialidad
        {
            get { return Doctor.Especialidad; } // Se accede desde el doctor
        }

        public CitaMedica(int id, Paciente paciente, Doctor doctor, DateTime horario)
        {
            Id = id;
            Paciente = paciente;
            Doctor = doctor;
            Horario = horario;
            Ticket = "TCK-" + id.ToString("D4") + "-" + paciente.DNI;
        }

        public override string ToString()
        {
            return "🎫 " + Ticket + " | " + Paciente.Nombre + " | " + Especialidad + " | " + Doctor.Nombre + " | " + Horario;
        }
    }

    class Program
    {
        // Listas principales del sistema
        static List<Paciente> pacientes = new List<Paciente>();
        static List<Doctor> doctores = new List<Doctor>();
        static List<Enfermera> enfermeras = new List<Enfermera>();
        static List<Especialidad> especialidades = new List<Especialidad>()
        {
            new Especialidad("Cardiología"),
            new Especialidad("Pediatría"),
            new Especialidad("Neurología"),
            new Especialidad("Dermatología")
        };
        static List<CitaMedica> citas = new List<CitaMedica>();
        static int idCita = 1;

        static void Main(string[] args)
        {
            int opcion;
            do
            {
                Console.WriteLine("\n=== SISTEMA HOSPITALARIO ===");
                Console.WriteLine("1. Registrar nuevo paciente");
                Console.WriteLine("2. Registrar nuevo doctor");
                Console.WriteLine("3. Registrar nueva enfermera");
                Console.WriteLine("4. Agendar cita médica");
                Console.WriteLine("5. Ver citas registradas");
                Console.WriteLine("0. Salir");
                Console.Write("Seleccione una opción: ");
                opcion = int.Parse(Console.ReadLine());

                switch (opcion)
                {
                    case 1: RegistrarPaciente(); break;
                    case 2: RegistrarDoctor(); break;
                    case 3: RegistrarEnfermera(); break;
                    case 4: AgendarCita(); break;
                    case 5: MostrarCitas(); break;
                }
            } while (opcion != 0);
        }

        // Función para registrar pacientes
        static void RegistrarPaciente()
        {
            Console.Write("Nombre: "); string nombre = Console.ReadLine();
            Console.Write("Historia Médica: "); string historia = Console.ReadLine();
            Console.Write("DNI: "); string dni = Console.ReadLine();
            pacientes.Add(new Paciente(nombre, historia, dni));
            Console.WriteLine("✅ Paciente registrado.");
        }

        // Función para registrar doctores
        static void RegistrarDoctor()
        {
            Console.Write("Nombre: "); string nombre = Console.ReadLine();
            Console.Write("DNI: "); string dni = Console.ReadLine();
            Console.Write("Código Médico: "); string codigo = Console.ReadLine();

            Console.WriteLine("Especialidades disponibles:");
            for (int i = 0; i < especialidades.Count; i++)
                Console.WriteLine("[" + (i + 1) + "] " + especialidades[i]);
            int opcion = int.Parse(Console.ReadLine()) - 1;
            var especialidad = especialidades[opcion];

            // Horarios disponibles (ficticios por ahora)
            List<DateTime> horarios = new List<DateTime>();
            horarios.Add(DateTime.Now.AddHours(1));
            horarios.Add(DateTime.Now.AddHours(2));

            doctores.Add(new Doctor(nombre, dni, codigo, especialidad, horarios));
            Console.WriteLine("✅ Doctor registrado.");
        }

        // Función para registrar enfermeras
        static void RegistrarEnfermera()
        {
            Console.Write("Nombre: "); string nombre = Console.ReadLine();
            Console.Write("DNI: "); string dni = Console.ReadLine();
            Console.Write("Código Enfermera: "); string codigo = Console.ReadLine();
            enfermeras.Add(new Enfermera(nombre, dni, codigo));
            Console.WriteLine("✅ Enfermera registrada.");
        }

        // Agendar una nueva cita médica
        static void AgendarCita()
        {
            Console.Write("DNI del paciente: ");
            string dni = Console.ReadLine();
            var paciente = pacientes.Find(p => p.DNI == dni);
            if (paciente == null) { Console.WriteLine("❌ Paciente no encontrado."); return; }

            Console.WriteLine("Especialidades disponibles:");
            for (int i = 0; i < especialidades.Count; i++)
                Console.WriteLine("[" + (i + 1) + "] " + especialidades[i]);
            int opcion = int.Parse(Console.ReadLine()) - 1;
            var especialidad = especialidades[opcion];

            var doctoresEsp = doctores.FindAll(delegate (Doctor d) { return d.Especialidad == especialidad; });
            if (doctoresEsp.Count == 0) { Console.WriteLine("❌ No hay doctores disponibles para esta especialidad."); return; }

            Console.WriteLine("Doctores disponibles:");
            for (int i = 0; i < doctoresEsp.Count; i++)
                Console.WriteLine("[" + (i + 1) + "] " + doctoresEsp[i]);
            int docSel = int.Parse(Console.ReadLine()) - 1;
            var doctor = doctoresEsp[docSel];

            Console.WriteLine("Horarios disponibles:");
            for (int i = 0; i < doctor.HorariosDisponibles.Count; i++)
                Console.WriteLine("[" + (i + 1) + "] " + doctor.HorariosDisponibles[i]);
            int horSel = int.Parse(Console.ReadLine()) - 1;
            var horario = doctor.HorariosDisponibles[horSel];

            var cita = new CitaMedica(idCita++, paciente, doctor, horario);
            citas.Add(cita);
            doctor.HorariosDisponibles.RemoveAt(horSel); // Eliminar el horario ya reservado
            Console.WriteLine("✅ Cita agendada:");
            Console.WriteLine(cita);
        }

        // Mostrar todas las citas registradas
        static void MostrarCitas()
        {
            Console.WriteLine("\n=== CITAS REGISTRADAS ===");
            if (citas.Count == 0)
            {
                Console.WriteLine("❌ NO HAY CITAS REGISTRADAS AÚN.");
                return;
            }
            foreach (var cita in citas)
                Console.WriteLine(cita);
        }
    }
}
