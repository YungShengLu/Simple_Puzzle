using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Puzzle {
    public struct Position {
        public int x, y;
    };

    public partial class Main : Form {
        private string player;
        private Image[] image;
        private PuzzleBlock[,] puzzle;
        private PictureBox[,] table;
        private int[] randomList;
        private Random rand;
        private TimeSpan start;

        public Main() {
            InitializeComponent();

            // Initialize.
            image = new Image[9];
            table = new PictureBox[3, 3];
            randomList = new int[9];
            rand = new Random();
            start = new TimeSpan(0);            

            puzzle = new PuzzleBlock[3, 3];
            Position currPosition = new Position();
            for (int i = 0; i < 3; ++i) {
                for (int j = 0; j < 3; ++j) {
                    currPosition.x = i;
                    currPosition.y = j;

                    puzzle[i, j] = new PuzzleBlock();
                    puzzle[i, j].setPosition(currPosition);
                }
            }

            table[0, 0] = puzzle_11;
            table[0, 1] = puzzle_12;
            table[0, 2] = puzzle_13;
            table[1, 0] = puzzle_21;
            table[1, 1] = puzzle_22;
            table[1, 2] = puzzle_23;
            table[2, 0] = puzzle_31;
            table[2, 1] = puzzle_32;
            table[2, 2] = puzzle_33;

            // Subscribe.
            for (int i = 0; i < 3; ++i) {
                for (int j = 0; j < 3; ++j) {
                    puzzle[i, j].setPuzzle(table[i, j]);
                    puzzle[i, j].getPuzzle().Click += puzzle_Click;
                }
            }

            image[0] = Resource1._1;
            image[1] = Resource1._2;
            image[2] = Resource1._3;
            image[3] = Resource1._4;
            image[4] = Resource1._5;
            image[5] = Resource1._6;
            image[6] = Resource1._7;
            image[7] = Resource1._8;
            image[8] = Resource1._9;
        }

        private void Main_Load(object sender, EventArgs e) {
            label_timer.Text = "00:00:00";

            for (int i = 0; i < 3; ++i) {
                for (int j = 0; j < 3; ++j)
                    puzzle[i, j].setImage(image[8]);
            }
        }

        private void puzzle_Click(object sender, EventArgs e) {
            PictureBox curr = (PictureBox)sender;
            Position currPosition = getCurrPosition(curr);

            Console.WriteLine("{0} {1}", currPosition.x, currPosition.y);

            if (currPosition.x + 1 < 3 && puzzle[currPosition.x + 1, currPosition.y].getImage() == image[8]) {
                puzzle[currPosition.x + 1, currPosition.y].setImage(curr.Image);
                puzzle[currPosition.x, currPosition.y].setImage(image[8]);
            }
            else if (currPosition.x - 1 >= 0 && puzzle[currPosition.x - 1, currPosition.y].getImage() == image[8]) {
                puzzle[currPosition.x - 1, currPosition.y].setImage(curr.Image);
                puzzle[currPosition.x, currPosition.y].setImage(image[8]);
            }
            else if (currPosition.y + 1 < 3 && puzzle[currPosition.x, currPosition.y + 1].getImage() == image[8]) {
                puzzle[currPosition.x, currPosition.y + 1].setImage(curr.Image);
                puzzle[currPosition.x, currPosition.y].setImage(image[8]);
            }
            else if (currPosition.y - 1 >= 0 && puzzle[currPosition.x, currPosition.y - 1].getImage() == image[8]) {
                puzzle[currPosition.x, currPosition.y - 1].setImage(curr.Image);
                puzzle[currPosition.x, currPosition.y].setImage(image[8]);
            }
        }

        public Position getCurrPosition(PictureBox curr) {
            Position currPosition = new Position();

            for (int i = 0; i < 3; ++i) {
                for (int j = 0; j < 3; ++j) {
                    if (puzzle[i, j].getImage() == curr.Image) {
                        currPosition = puzzle[i, j].getPosition();
                        return currPosition;
                    }
                }
            }

            return currPosition;
        }

        private bool isFinished() {
            for (int i = 0; i < 3; ++i) {
                for (int j = 0; j < 3; ++j) {
                    if (puzzle[i, j].getImage() != image[i * 3 + j])
                        return false;
                }
            }

            return true;
        }

        private void button1_Click(object sender, EventArgs e) {
            player = textBox1.Text;

            while (player == "") {
                MessageBox.Show("Please enter your name.", "Puzzle");
                return;
            }

            // Random the puzzle.
            for (int i = 0; i < 9; ++i) {
                randomList[i] = rand.Next(1, 10);
                for (int j = 0; j < i; ++j) {
                    while (randomList[i] == randomList[j]) {
                        j = 0;
                        randomList[i] = rand.Next(1, 10);
                    }
                }
            }

            // Test for finishing.
            
            for (int i = 0; i < 7; ++i) {
                randomList[i] = i + 1;
            }

            randomList[7] = 9;
            randomList[8] = 8;
            

            // Set the puzzle.
            for (int i = 0; i < 3; ++i) {
                for (int j = 0; j < 3; ++j) {
                    puzzle[i, j].setImage(image[randomList[i * 3 + j] - 1]);
                    Console.Write("{0} ", randomList[i * 3 + j] - 1);
                }
                Console.WriteLine("");
            }

            // Set timer.
            timer.Enabled = true;
        }

        private void timer_Tick(object sender, EventArgs e) {
            start += TimeSpan.FromMilliseconds(100);
            label_timer.Text = start.ToString(@"hh\:mm\:ss");

            DialogResult result = new DialogResult();
            if (isFinished()) {
                timer.Stop();

                result = MessageBox.Show("Finish!", "Puzzle", MessageBoxButtons.OK);
                if (result == DialogResult.OK) {
                    textBox1.Text = "";
                    listBox1.Items.Add(player + "  " + label_timer.Text);
                }

                label_timer.Text = "00:00:00";
                timer.Enabled = false;
                timer.Interval = 100;
                start = new TimeSpan(0);

                for (int i = 0; i < 3; ++i) {
                    for (int j = 0; j < 3; ++j)
                        puzzle[i, j].setImage(image[8]);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e) {
            DialogResult result = MessageBox.Show("Puzzle", "Are you sure to delete the record?", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes) {
                listBox1.Items.Remove(listBox1.SelectedItem);
            }
        }
    }

    public class PuzzleBlock {
        private PictureBox picturebox;
        private Position position;

        public PuzzleBlock() {
            picturebox = new PictureBox();
            position = new Position();
        }

        public void setImage(Image image) {
            this.picturebox.Image = image;
        }

        public Image getImage() {
            return this.picturebox.Image;
        }

        public void setPuzzle(PictureBox picturebox) {
            this.picturebox = picturebox;
        }

        public PictureBox getPuzzle() {
            return this.picturebox;
        }

        public void setPosition(Position curr_position) {
            this.position = curr_position;
        }

        public Position getPosition() {
            return this.position;
        }
    }
}