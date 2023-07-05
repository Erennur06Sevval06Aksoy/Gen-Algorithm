using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gen_Algoritması
{
    public partial class Form3 : Form
    {
        private List<string> sequences;
        private Random random = new Random();
        private int sequences_count;
        int gap_penalty = 0;
        int match_score = 1;
        int mismatch_penalty = -1;
        int crossover_step = 5;
        public Form3(List<string> values)
        {
            InitializeComponent();
            sequences = values;
        }
        //Seçilen diziler arasında ikili hizalama yapmak için needleman wunsch algoritması kullanıldı
        int CalculateNeedlemanWunsch(int[] sequence1, int[] sequence2)
        {
            int[,] selected_sequence = new int[sequence1.Length + 1, sequence2.Length + 1];

            for (int i = 0; i <= sequence1.Length; i++)
                selected_sequence[i, 0] = i * gap_penalty;

            for (int j = 0; j <= sequence2.Length; j++)
                selected_sequence[0, j] = j * gap_penalty;

            for (int i = 1; i <= sequence1.Length; i++)
            {
                for (int j = 1; j <= sequence2.Length; j++)
                {
                    int match = selected_sequence[i - 1, j - 1] + (sequence1[i - 1] == sequence2[j - 1] ? match_score : mismatch_penalty);
                    int x1 = selected_sequence[i - 1, j] + gap_penalty;
                    int x2 = selected_sequence[i, j - 1] + gap_penalty;

                    selected_sequence[i, j] = Math.Max(Math.Max(match, x1), x2);
                }
            }
            return selected_sequence[sequence1.Length, sequence2.Length];
        }

        //Seçilen değerler üzerinde çaprazlama işlemi uygulanıyor
        private int[] Crossover(int[] parent1, int[] parent2)
        {
            int crossover_point = random.Next(1, parent1.Length - 1);
            int[] child = new int[parent1.Length];

            //parent1 dizisinden child dizisine crossover_point noktasına kadar olan elemanları kopyalar
            Array.Copy(parent1, child, crossover_point);
            //parent2 dizisinden crossover_point noktasından itibaren kalan elemanları child dizisine kopyalar
            Array.Copy(parent2, crossover_point, child, crossover_point, parent2.Length - crossover_point);
            return child;
        }

        private void CrossoverStep(int[][] asciiCodes)
        {
            //asciiCodes dizisindeki ebeveynlerin dizinlerini belirlemek için üretiliyor
            int parent1_index = random.Next(0, asciiCodes.Length);
            int parent2_index = random.Next(0, asciiCodes.Length);

            //parent1_index ve parent2_index değerlerinin birbirinden farklı olmasını sağlar
            while (parent2_index == parent1_index)
                parent2_index = random.Next(0, asciiCodes.Length);

            int[] parent1 = asciiCodes[parent1_index];
            int[] parent2 = asciiCodes[parent2_index];
            int[] child = Crossover(parent1, parent2);
            //child dizisinin yerine geçecek dizinin indexini belirlemek için rastgele bir sayı üretiliyor
            int replace_index = random.Next(0, asciiCodes.Length);
            asciiCodes[replace_index] = child;

            listBox2.Items.Add("Çaprazlama Sonucu:");
            listBox2.Items.Add("Parent 1: " + string.Join(", ", parent1.Select(x => (char)x)));
            listBox2.Items.Add("Parent 2: " + string.Join(", ", parent2.Select(x => (char)x)));
            listBox2.Items.Add("Child: " + string.Join(", ", child.Select(x => (char)x)));
            listBox2.Items.Add("-----------------------------------");
        }
        //Üretilen çocuklardan rastgele birisine mutasyon işlemi uygular
        private void MutationStep(int[][] asciiCodes)
        {
            int mutation_index = random.Next(0, asciiCodes.Length);
            int[] mutation_child = asciiCodes[mutation_index];

            int mutation_point = random.Next(0, mutation_child.Length);
            //mutation_value 97 ile 123 arasında bir sayıdır ve ASCII tablosundaki bir harfi temsil ediyor
            int mutation_value = random.Next(97, 123);

            mutation_child[mutation_point] = mutation_value;

            listBox2.Items.Add("Mutasyon Sonucu:");
            listBox2.Items.Add("New Child: " + string.Join(", ", mutation_child.Select(x => (char)x)));
            listBox2.Items.Add("Mutation Index: " + mutation_index);
            listBox2.Items.Add("Mutation Point: " + mutation_point);
            listBox2.Items.Add("Mutation Value: " + (char)mutation_value);
            listBox2.Items.Add("-----------------------------------");
            listBox1.Items.Add(string.Join("", mutation_child.Select(x => (char)x)));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (string sequence in sequences)
            {
                listBox1.Items.Add(sequence);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] array_sequences = new string[listBox1.Items.Count];
            //listboxde listelenen popülasyın array_sequences dizisinde toplanıyor
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                array_sequences[i] = listBox1.Items[i].ToString();
            }

            //Çoklu hizalama algoritması olduğu için en az 3 DNA dizisi girilmelidir
            if (sequences.Count < 3)
            {
                MessageBox.Show("En az 3 DNA dizisi ekleyin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Form1 form1 = new Form1();
                form1.Show();
                sequences.Clear();
                return;
            }

            // Random olarak seçilecek dizi sayısı belirleniyor
            if (sequences.Count % 2 == 0)
                sequences_count = sequences.Count / 2;
            else
                sequences_count = (sequences.Count + 1) / 2;

            // Random olarak seçilen dizileri tutacak bir liste oluşturuluyor
            List<string> randomSelected = new List<string>();

            // Karşılaştırma yapmak istediğiniz dizilerin sayısı kadar döngü oluşturuluyor
            for (int i = 0; i < sequences_count; i++)
            {
                // Random olarak bir diziyi seçin
                int random_index = random.Next(0, sequences.Count);
                string selectedSequence = sequences[random_index];

                // Seçilen dizinin daha önce seçilmemiş olması sağlanıyor
                while (randomSelected.Contains(selectedSequence))
                {
                    random_index = random.Next(0, sequences.Count);
                    selectedSequence = sequences[random_index];
                }
                // Seçilen diziyi randomSelected listesine ekleniyor
                randomSelected.Add(selectedSequence);
            }

            // Hizalama işlemi için dizi karakterlerini ASCII koduna dönüştürülüyor
            List<int[]> ascii_codes_list = new List<int[]>();

            foreach (string sequence in randomSelected)
            {
                int[] sequence_ascii_codes = new int[sequence.Length];

                for (int i = 0; i < sequence.Length; i++)
                {
                    sequence_ascii_codes[i] = (int)sequence[i];
                }
                ascii_codes_list.Add(sequence_ascii_codes);
            }

            //Seçilen diziler arasında calculate_needleman_wunsch metodu yardımıyla hizalama yapılıyor
            int[][] ascii_codes = ascii_codes_list.ToArray();

            //Çaprazlama işlemi ve mutasyon işlemi iterasyon sayısı kadar tekrarlanıyor
            int iterasyon = Convert.ToInt16(textBox1.Text);
            for (int i = 0; i < iterasyon; i++)
            {
                for (int j = 0; j < crossover_step; j++)
                {
                    CrossoverStep(ascii_codes);
                }
                MutationStep(ascii_codes);
            }
        }
    }
}