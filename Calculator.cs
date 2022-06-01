using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVGB07_lab2
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
        }

        // funktion som nollställer miniräknaren
        private void clearCalculator()
        {
            txtResult.Clear();
            btnEqualsIsPressed = false;
            btnCalcIsPressed = false;
            nextNrForCalc = false;
            result = 0;
            prevInput.Text = "";

            calc[0] = "";
            calc[1] = "";
            calc[2] = "";
        }

        // funktion som beräknar resultatet av talen i "calc"-vektorn
        private void calculate()
        {
            try
            {
                switch (calc[1])
                {
                    case "+":
                        result = long.Parse(calc[0]) + long.Parse(calc[2]);
                        txtResult.Text = $"{result}";
                        break;
                    case "-":
                        result = long.Parse(calc[0]) - long.Parse(calc[2]);
                        txtResult.Text = $"{result}";
                        break;
                    case "/":
                        // om man delar med 0 får man ett felmeddelande och miniräknaren nollställs
                        if (calc[2] != "0")
                        {
                            result = long.Parse(calc[0]) / long.Parse(calc[2]);
                            txtResult.Text = $"{result}";
                        }
                        else
                        {
                            MessageBox.Show("Invalid calculation!");
                            clearCalculator();
                        }
                        break;
                    case "x":
                        result = long.Parse(calc[0]) * long.Parse(calc[2]);
                        txtResult.Text = $"{result}";
                        break;
                    default:
                        break;
                }
            }
            catch (OverflowException)
            {
                MessageBox.Show($"Value too large/small!");
                clearCalculator();
            }
        }

        private void btnNr_Click(object sender, EventArgs e)
        {
            // hämtar siffran (0-9) som korresponderar med aktuellt objekt
            nr = int.Parse(sender.ToString().Last().ToString());

            // om föregående klickad knapp är "=" nollställs miniräknaren
            if (btnEqualsIsPressed)
            {
                clearCalculator();
            }

            // lägger in siffror i sträng-vektorn "calc"
            // (calc[0] <= tal 1), (calc[1] <= operator), (calc[2] <= tal 2)
            if (!nextNrForCalc)
            {
                calc[0] += $"{nr}";
                txtResult.Text = calc[0];
            }
            else
            {
                calc[2] += $"{nr}";
                txtResult.Text = calc[2];
            }

            btnCalcIsPressed = false;
            btnEqualsIsPressed = false;
        }

        private void btnCalc_Click(object sender, EventArgs e)
        {
            // hämtar operatorn (/,*,-,+) som korresponderar med aktuellt objekt
            calcOperator = sender.ToString().Last().ToString();

            // om föregående klickad knapp är "="-knappen påbörjas en ny beräkning
            // med resultatet som första tal
            if (btnEqualsIsPressed)
            {
                calc[0] = txtResult.Text;
                calc[2] = "";
            }

            // skriver ut innehållet i "calc"-vaktorn ovanför resultatrutan
            if (btnCalcIsPressed)
            {
                string t = prevInput.Text;
                t = t.Remove((t.Length) - 2, 2);
                t = t.Insert(t.Length, $"{calcOperator} ");
                prevInput.Text = t;
            }
            else if (!calc[2].Equals(""))
            {
                prevInput.Text += $"{calc[2]} {calcOperator} ";
            }
            else
            {
                prevInput.Text += $"{calc[0]} {calcOperator} ";
            }

            // om det finns två tal i vektorn utförs beräkningen
            if (!calc[0].Equals("") && !calc[1].Equals("") && !calc[2].Equals(""))
            {
                calculate();

                // resultatet används som första talet i nästa beräkning
                calc[0] = $"{result}";
                calc[2] = "";
            }

            // lägger till aktuell operator i vektorn "calc"
            calc[1] = calcOperator;

            // tilldelar med "true" så att nästa tal i beräkningen kan läggas till
            // i vektorn
            nextNrForCalc = true;

            btnCalcIsPressed = true;
            btnEqualsIsPressed = false;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clearCalculator();
        }

        private void btnEquals_Click(object sender, EventArgs e)
        {
            if (!calc[0].Equals("") && !calc[1].Equals(""))
            {
                // om andra talet saknas används första talet på båda positionerna för att
                // utföra beräkningen
                if (calc[2].Equals(""))
                {
                    calc[2] = calc[0];
                }
                calculate();
            }

            // resultatet används som första tal i nästa beräkning
            calc[0] = $"{result}";

            prevInput.Text = "";

            btnEqualsIsPressed = true;
            btnCalcIsPressed = false;
        }

        long result = 0;
        int nr;
        string calcOperator;
        string[] calc = {"", "", ""};
        bool btnEqualsIsPressed = false;
        bool nextNrForCalc = false;
        bool btnCalcIsPressed = false;
    }
}
