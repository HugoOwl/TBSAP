using Aula06_Exercicio_.Business;
using Aula06_Exercicio_.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aula06_Exercicio_
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btLogin_Click(object sender, EventArgs e)
        {
            SAPClass sapClass = new SAPClass(txtURL.Text, txtServer.Text);

            try
            {
                LoginModel login = new LoginModel();

                login.CompanyDB = txtDb.Text;
                login.UserName = txtUser.Text;
                login.Password = txtPassword.Text;


                txtDIAPI.Text = sapClass.ConnectToDi(login);
                txtSession.Text = sapClass.SAPLogin(login);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btInserirPN_Click(object sender, EventArgs e)
        {
            SAPClass sClass = new SAPClass();

            try
            {
                sClass.InsertPN(txtCodigoPN.Text, txtNomePN.Text);

                MessageBox.Show("PN Inserido com sucesso");

                txtCardCode.Text = txtCodigoPN.Text;
                txtCardName.Text = txtNomePN.Text;

                txtCodigoPN.Clear();
                txtNomePN.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btInserirItem_Click(object sender, EventArgs e)
        {
            SAPClass sClass = new SAPClass();

            try
            {
                sClass.InsertItem(txtCodigoItem.Text, txtNomeItem.Text);

                MessageBox.Show("Item Inserido com sucesso");

                gridItems.Rows.Add(txtCodigoItem.Text,"1","10");

                txtCodigoItem.Clear();
                txtNomeItem.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            SAPClass sapClass = new SAPClass(txtURL.Text, txtServer.Text);

            try
            {
                OrderModel oOrder = new OrderModel();

                oOrder.CardCode = txtCardCode.Text;
                oOrder.DocDate = DateTime.Now;
                oOrder.DocDueDate = DateTime.Now;
                oOrder.TaxDate = DateTime.Now;
                oOrder.Comments = "Meu primeiro Pedido via Service Layer";

                foreach (DataGridViewRow linha in gridItems.Rows)
                {
                    ItemModel oItem = new ItemModel();

                    if (linha.Cells[0].Value != null)
                    {
                        oItem.ItemCode = linha.Cells[0].Value.ToString();
                        oItem.Quantity = linha.Cells[1].Value.ToString();
                        oItem.UnitPrice = linha.Cells[2].Value.ToString();

                        oOrder.DocumentLines.Add(oItem);
                    }
                }

                txtDocNum.Text = sapClass.InserirPedido(oOrder, txtSession.Text).DocNum;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
