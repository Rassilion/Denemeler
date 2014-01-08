using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ForgeBot
{
    public partial class CoordsForm : Form
    {
        private ApplicationSettings _settings;
        public CoordsForm(ApplicationSettings settings)
        {
            _settings = settings;
            InitializeComponent();
        }

        private void CoordsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //Save
                ItemCollection l_ItemCollection = ItemCollection.Instance;
                l_ItemCollection.Items.Clear();
                string l_CoordinateString;//Format xxxxyyyy

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    //ItemCollection.Item l_Item = new ItemCollection.Item();
                    if (dataGridView1.Rows[i].Cells[0].Value != null)
                    {
                        l_CoordinateString = dataGridView1.Rows[i].Cells[0].Value.ToString();
                        l_ItemCollection.Items[l_CoordinateString] = new ItemCollection.Item(int.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString()), int.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString()), dataGridView1.Rows[i].Cells[5].Value.ToString(), int.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString()), bool.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString()));
                    }
                    else
                    {
                        break;
                    }
                }
                saveDataToSettings();
            }
            catch (Exception)
            {

            }
        }

        public void loadData()
        {
            ItemCollection l_ItemCollection = ItemCollection.Instance;

            int l_Index = 0;
            dataGridView1.Rows.Clear();
            // Loop over pairs with foreach
            foreach (KeyValuePair<string, ItemCollection.Item> pair in l_ItemCollection.Items)
            {
                l_Index = dataGridView1.Rows.Add();
                dataGridView1.Rows[l_Index].Cells[0].Value = pair.Key;
                dataGridView1.Rows[l_Index].Cells[1].Value = pair.Value.m_OffsetX;
                dataGridView1.Rows[l_Index].Cells[2].Value = pair.Value.m_OffsetY;
                dataGridView1.Rows[l_Index].Cells[5].Value = pair.Value.timestamp;
                dataGridView1.Rows[l_Index].Cells[3].Value = pair.Value.YStepping;
                dataGridView1.Rows[l_Index].Cells[4].Value = pair.Value.LastClickWasSuccessful;
            }
            
        }

        public void saveDataToSettings()
        {
            try
            {
                dataGridView1.Rows.Clear();

                _settings.CoordinateData = ItemCollection.BuildCoordinateData();
                _settings.SaveSettings();
            }
            catch (Exception)
            {

            }
        }

        public void loadDataFromSettings()
        {
            ItemCollection.LoadInstanceFromCoordinateData(_settings.CoordinateData);
        }
    }
}
