using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthcareSystem
{
    public class Repository<T>
    {
        private List<T> items = new List<T>();

        public void Add(T item)
        {
            items.Add(item);
        }

        public List<T> GetAll()
        {
            return items;
        }

        public T GetById(Func<T, bool> predicate)
        {
            return items.FirstOrDefault(predicate);
        }

        public bool Remove(Func<T, bool> predicate)
        {
            T item = items.FirstOrDefault(predicate);

            if (item != null)
            {
                items.Remove(item);
                return true;
            }

            return false;
        }
    }

    public class Patient
    {
        public int Id;
        public string Name;
        public int Age;
        public string Gender;

        public Patient(int id, string name, int age, string gender)
        {
            Id = id;
            Name = name;
            Age = age;
            Gender = gender;
        }
    }

    public class Prescription
    {
        public int Id;
        public int PatientId;
        public string MedicationName;
        public DateTime DateIssued;

        public Prescription(
            int id,
            int patientId,
            string medicationName,
            DateTime dateIssued)
        {
            Id = id;
            PatientId = patientId;
            MedicationName = medicationName;
            DateIssued = dateIssued;
        }
    }

    public class HealthSystemApp
    {
        private Repository<Patient> _patientRepo = new Repository<Patient>();
        private Repository<Prescription> _prescriptionRepo = new Repository<Prescription>();

        private Dictionary<int, List<Prescription>>
            _prescriptionMap = new Dictionary<int, List<Prescription>>();

        public void SeedData()
        {
            _patientRepo.Add(new Patient(1, "John", 25, "Male"));
            _patientRepo.Add(new Patient(2, "Mary", 30, "Female"));
            _patientRepo.Add(new Patient(3, "Peter", 45, "Male"));

            _prescriptionRepo.Add(
                new Prescription(1, 1, "Paracetamol", DateTime.Now));

            _prescriptionRepo.Add(
                new Prescription(2, 1, "Amoxicillin", DateTime.Now));

            _prescriptionRepo.Add(
                new Prescription(3, 2, "Ibuprofen", DateTime.Now));

            _prescriptionRepo.Add(
                new Prescription(4, 3, "Vitamin C", DateTime.Now));

            _prescriptionRepo.Add(
                new Prescription(5, 2, "Cetirizine", DateTime.Now));
        }

        public void BuildPrescriptionMap()
        {
            foreach (var p in _prescriptionRepo.GetAll())
            {
                if (!_prescriptionMap.ContainsKey(p.PatientId))
                {
                    _prescriptionMap[p.PatientId] =
                        new List<Prescription>();
                }

                _prescriptionMap[p.PatientId].Add(p);
            }
        }

        public List<Prescription> GetPrescriptionsByPatientId(int patientId)
        {
            return _prescriptionMap.ContainsKey(patientId)
                ? _prescriptionMap[patientId]
                : new List<Prescription>();
        }

        public void PrintAllPatients()
        {
            foreach (var p in _patientRepo.GetAll())
            {
                Console.WriteLine(
                    $"{p.Id} - {p.Name} - {p.Age} - {p.Gender}");
            }
        }

        public void PrintPrescriptionsForPatient(int id)
        {
            foreach (var p in GetPrescriptionsByPatientId(id))
            {
                Console.WriteLine(
                    $"{p.MedicationName} ({p.DateIssued:d})");
            }
        }
    }

    class Program
    {
        static void Main()
        {
            HealthSystemApp app = new HealthSystemApp();

            app.SeedData();
            app.BuildPrescriptionMap();

            app.PrintAllPatients();

            Console.WriteLine("\nPrescriptions for Patient 1");
            app.PrintPrescriptionsForPatient(1);
        }
    }
}