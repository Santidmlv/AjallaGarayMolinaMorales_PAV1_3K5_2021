﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TP_Grupo5.DataAccesLayer;
using TP_Grupo5.BusinesLayer;
using TP_Grupo5.Entities;
using TP_Grupo5.GUILayer;

namespace TP_Grupo5
{
    public partial class frmConsultaCliente : Form
    {
        private BarrioServicio oBarrioServicio;
        private ClienteServicio oClienteServicio;
        public frmConsultaCliente()
        {
            InitializeComponent();
            oBarrioServicio = new BarrioServicio();
            oClienteServicio = new ClienteServicio();
            LlenarCombo(cboBarrio,oBarrioServicio.dameTodo(),"Nombre","Id_barrio");
            habilitarCampos(true);
        }
        private void LlenarCombo(ComboBox cbo, Object source, string display, String value)
        {
            cbo.DataSource = source;
            cbo.DisplayMember = display;
            cbo.ValueMember = value;
            cbo.SelectedIndex = -1;
        }


        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string filtro = string.Empty;

            if (!chkTodos.Checked)
            {
                if (txtRazonSocial.Text != string.Empty)
                {
                    filtro = filtro + " AND c.razon_social LIKE '%" + txtRazonSocial.Text + "%'";
                }
                if (cboBarrio.SelectedIndex != -1)
                {
                    filtro = filtro + " AND c.id_barrio=" + cboBarrio.SelectedValue;
                }
                if (txtCuit.Text != string.Empty)
                {
                    filtro = filtro + " AND c.cuit=" + Convert.ToInt32(txtCuit.Text);
                }
                //fecha

                llenarGrilla(grdClientes, oClienteServicio.consultaConFiltros(filtro));
            }
            else
                llenarGrilla(grdClientes, oClienteServicio.dameTodo());

        }

        private void habilitarCampos(Boolean valor)
        {
            txtRazonSocial.Text = string.Empty;
            txtCuit.Text = string.Empty;
            cboBarrio.SelectedIndex = -1;
            grdClientes.Rows.Clear();
            btnActualizar.Enabled = !valor;
            btnEliminar.Enabled = !valor;
        }

        private void llenarGrilla(DataGridView grilla, IList<Cliente> lista)
        {
            grilla.Rows.Clear();
            if (lista.Count > 0)
            {
                for (int i = 0; i < lista.Count; i++)
                {
                    grdClientes.Rows.Add(
                        lista[i].Id_cliente,
                        lista[i].Razon_social,
                        lista[i].Cuit,
                        lista[i].Barrio.Nombre,
                        lista[i].Calle,
                        lista[i].Numero,
                        lista[i].Fecha_alta,
                        lista[i].Contacto.Nombre
                        );
                }
                btnActualizar.Enabled = true;
                btnEliminar.Enabled = true;
            }
            else
            {
                MessageBox.Show("No se encontraron Clientes", "Notificación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnEliminar.Enabled = false;
                btnActualizar.Enabled = false;
            }
                
        }

        private void chkTodos_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTodos.Checked)
                gbBuscarCliente.Enabled = false;
            else
                gbBuscarCliente.Enabled = true;
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            habilitarCampos(true);
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtCuit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else 
            if (Char.IsControl(e.KeyChar))
            {
                         e.Handled = false;
            }
            else if (Char.IsSeparator(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmABMCliente ofrmABMCliente = new frmABMCliente();
            ofrmABMCliente.ShowDialog();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (grdClientes.CurrentRow == null)
                MessageBox.Show("Seleccione una fila de la grilla");
            else
            {
                frmABMCliente ofrmABMCliente = new frmABMCliente();
                ofrmABMCliente.SeleccionarCliente(frmABMCliente.FormMode.update, (int)grdClientes.CurrentRow.Cells[0].Value);
                ofrmABMCliente.ShowDialog();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (grdClientes.CurrentRow == null)
                MessageBox.Show("Seleccione una fila de la grilla");
            else
            {
                frmABMCliente ofrmABMCliente = new frmABMCliente();
                ofrmABMCliente.SeleccionarCliente(frmABMCliente.FormMode.delete, (int)grdClientes.CurrentRow.Cells[0].Value);
                ofrmABMCliente.ShowDialog();
            }
        }

    }
}
