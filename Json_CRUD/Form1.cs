using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;
using System.Collections;

namespace Json_CRUD
{
    public partial class Form1 : Form
    {
        string p2;
        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All Files|*.*";
            dlg.Title = "Select File";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                p2 = dlg.FileName.ToString();
                label3.Text = p2;
            }

        }
        private void Display()
        {
            var json = File.ReadAllText(p2);
            try
            {
                var jObject = JObject.Parse(json);
                richTextBox2.AppendText(jObject.ToString());
                if (jObject != null)
                {
                    richTextBox1.AppendText("Name :" + jObject["name"].ToString()+"\n");
                    richTextBox1.AppendText("Address :" + jObject["address"].ToString()+"\n");
                    var address = jObject["Details"];
                    richTextBox1.AppendText("area :" + address["area"].ToString() + "\n");
                    richTextBox1.AppendText("Postalcode :" + address["postalcode"].ToString() + "\n");

                    JArray fac = (JArray)jObject["Faculties"];
                    if(fac!=null)
                    {
                        richTextBox1.AppendText("Faculties :");
                        foreach(var item in fac)
                        {
                            richTextBox1.AppendText("\n\t"+item.ToString());
                        }
                    }
                    richTextBox1.AppendText("\n");
                    JArray std = (JArray)jObject["students"];
                    if(std!=null)
                    {
                        foreach(var item in std)
                        {
                            richTextBox1.AppendText("Faculty :" + item["fac"].ToString() + ",\t");
                            richTextBox1.AppendText("Person :" + item["person"].ToString() + "\n");
                        }
                    }

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Wrong JSON File selected!\n\t"+ex.Message);
            }  
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Display();
            setAlldata();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
     
        private void Add(string x)
        {
            var fac = textBox1.Text;
            var person = textBox2.Text;
            var neww = "{ 'fac': '" + fac + "', 'person': " + person + "}";
            var newdata ="";
          
         
           
            
            MessageBox.Show("\"" + textBox1.Text.ToString() + "\"");
            try
            {
                var json = File.ReadAllText(p2);
                var jsonObj = JObject.Parse(json);

                var facarray = jsonObj.GetValue(x) as JArray;

                MessageBox.Show(facarray.ToString());
                if(x == "students")
            {
               var  newdata2 = JObject.Parse(neww);
               facarray.Add(newdata2);

            }
                else if (x == "Faculties")
                {
                    newdata = fac.ToString();
                    facarray.Add(newdata);
                }

                jsonObj[x] = facarray;

                string newJsonResult = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);

                File.WriteAllText(p2, newJsonResult);
                MessageBox.Show("Added Successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Add Error : " + ex.Message.ToString());
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Add(this.comboBox1.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = String.Empty;
            richTextBox2.Text = String.Empty;
            comboBox2.Items.Clear();
            textBox6.Text = String.Empty;
            textBox7.Text = String.Empty;

        }



        void setAlldata()
        {
            string x = "";
            var json = File.ReadAllText(p2);
            try
            {
                var jObject = JObject.Parse(json);
                dynamic stuff = JsonConvert.DeserializeObject(json);

                textBox3.Text = stuff.name;
                textBox4.Text = stuff.address;
                textBox5.Text = stuff.Details.area;
                textBox8.Text = stuff.Details.postalcode;
               // MessageBox.Show((stuff.Faculties.Count).ToString());
                ArrayList arr = new ArrayList();

                for(int i = 0 ; i<stuff.Faculties.Count ; i++)
                {
                    arr.Add(stuff.Faculties[i].ToString());
                        
                }
                comboBox2.Items.AddRange(arr.ToArray());

            }
            catch (Exception ex)
            {

                MessageBox.Show("Wrong JSON File selected!\n\t" + ex.Message);
            }  
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string texts = comboBox2.GetItemText(comboBox2.SelectedItem);
            int indexx = comboBox2.SelectedIndex;
          //  MessageBox.Show(indexx.ToString());

            var json = File.ReadAllText(p2);
            try
            {
                var jObject = JObject.Parse(json);
                dynamic stuff = JsonConvert.DeserializeObject(json);

                //MessageBox.Show(stuff.students[indexx].fac.ToString() + "\n" + stuff.students[indexx].person.ToString());
                textBox6.Text = stuff.students[indexx].fac.ToString();
                textBox7.Text = stuff.students[indexx].person.ToString();
            }
            catch (Exception)
            {

                throw;
            }  

        }

        void update()
        {
            //   string x="";
            string uname = this.textBox3.Text;
            string uaddress = this.textBox4.Text;
            string uarea = this.textBox5.Text;
            string upostalcode = this.textBox8.Text;
            string ufac = this.textBox6.Text;
            string uperson = this.textBox7.Text;
            try
            {
                int indexx = comboBox2.SelectedIndex;
                var json = File.ReadAllText(p2);
                var jsonObj = JObject.Parse(json);
                dynamic stuff = JsonConvert.DeserializeObject(json);

                stuff.name = uname.ToString();
                stuff.address = uaddress;
                stuff.Details.area = uarea;
                stuff.Details.postalcode = upostalcode;
                stuff.students[indexx].fac = ufac;
                stuff.students[indexx].person = uperson;
                stuff.Faculties[indexx] = ufac;

                string newJsonResult = JsonConvert.SerializeObject(stuff, Formatting.Indented);
                File.WriteAllText(p2, newJsonResult);

                MessageBox.Show("Updated!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Add Error : " + ex.Message.ToString());
            }  
        }

        private void button5_Click(object sender, EventArgs e)
        {
            update();
         
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
          
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Delete();
           // DeleteStudent();
        }
        void Delete()
        {
            var json = File.ReadAllText(p2);
            try
            {
                var jObject = JObject.Parse(json);
                JArray FacultyArray = (JArray)jObject["Faculties"];
                JArray studentArray = (JArray)jObject["students"];
                var Name = this.textBox10.Text.ToString();


                var StudentDeleted = studentArray.FirstOrDefault(obj => obj["fac"].Value<String>() == Name.ToString());
                var FacultyoDeleted = FacultyArray.FirstOrDefault(obj => obj.Value<String>() == Name.ToString());

                    FacultyArray.Remove(FacultyoDeleted);
                    studentArray.Remove(StudentDeleted);

                    string output = Newtonsoft.Json.JsonConvert.SerializeObject(jObject, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText(p2, output);
                    MessageBox.Show("Deleted Successfully!!");
              
            }
            catch (Exception)
            {

                throw;
            } 
        }
     
        private void button6_Click(object sender, EventArgs e)
        {
            var json = File.ReadAllText(p2);
            try
            {
                var jObject = JObject.Parse(json);
                richTextBox2.AppendText(jObject.ToString());
                if (jObject != null)
                {
                    JArray std = (JArray)jObject["students"];
                    if (std != null)
                    {
                        foreach (var item in std)
                        {
                            if (String.Equals(this.textBox9.Text.ToString(), item["fac"].ToString(), StringComparison.OrdinalIgnoreCase))
                            {
                                this.textBox10.Text = item["fac"].ToString();
                                this.textBox11.Text = item["person"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Wrong JSON File selected!\n\t" + ex.Message);
            }  
        }
    }







}


/*



*/