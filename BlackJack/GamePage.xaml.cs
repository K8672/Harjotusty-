using System;
using BlackJack.Common;
using BlackJack.GameManager;
using BlackJack.Players;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BlackJack
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class GamePage : Page
    {
        public Player player;
        public Manager gm;
        private static int startXPos = -10;
        private static int dealerStartXPos = 285;
        private int playerCardXPos = startXPos;
        private int dealerCardXPos = dealerStartXPos;
        public List<PictureBox> playerCardsToDisplay;
        public List<PictureBox> dealerCards;
        private object resultLabel;

        public GamePage()
        {
            this.InitializeComponent();
            gm = new Manager();
            player = new Player();
            gm.AddPlayer(player);
            playerCardsToDisplay = new List<PictureBox>();
            dealerCards = new List<PictureBox>();

            splitButton.Hide();
        }


        private void DrawPlayerCard(Card card)
        {
            playerCardXPos += 30;
            PictureBox newCard = new PictureBox();
            Image img = ImageSource("/Assets/" + card.Value + card.Suit + ".png");
            newCard.Image = img;
            newCard.Location = new System.Drawing.Point(playerCardXPos, 180);
            newCard.Name = "newCard";
            newCard.Size = new System.Drawing.Size(72, 99);
            this.Controls.Add(newCard);
            newCard.BringToFront();
            playerCardsToDisplay.Add(newCard);
        }

        private void DrawDealerCardNotShown(Card card)
        {

            dealerCardXPos -= 30;
            PictureBox blankCard = new PictureBox();
            Image blankImage = Image.FromFile("Assets/b1fv.png");
            blankCard.Image = blankImage;
            blankCard.Location = new System.Drawing.Point(dealerCardXPos, 12);
            blankCard.Name = "newCard";
            blankCard.Size = new System.Drawing.Size(72, 99);
            this.Controls.Add(blankCard);
            blankCard.BringToFront();
            dealerCards.Add(blankCard);

            DrawDealer(card);

        }

        private void DrawDealer(Card card)
        {
            dealerCardXPos -= 30;
            PictureBox newCard = new PictureBox();
            Image img = Image.FromFile("Assets/" + card.Value + card.Suit + ".png");
            newCard.Image = img;
            newCard.Location = new System.Drawing.Point(dealerCardXPos, 12);
            newCard.Name = "newCard";
            newCard.Size = new System.Drawing.Size(72, 99);
            this.Controls.Add(newCard);
            dealerCards.Add(newCard);
        }
        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            // get root frame
            Frame rootFrame = Window.Current.Content as Frame;
            // did we get it correctly
            if (rootFrame == null) return;
            // navigate back if possible
            if (rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
            }
        }

        private void hitMeButton_Click(object sender, RoutedEventArgs e)
        {
            if (!player.Busted)
            {
                player.HitMe(gm.DealCard());
                playerScore.Text = "Score: " + player.GetScore();
                DrawPlayerCard(player.LastCard());

                if (player.Busted)
                {
                    hitMeButton.Enabled = false;
                    standButton.Enabled = false;
                    splitButton.Enabled = false;
                    resultLabel.Text = "Dealer wins!";
                }
            }
        }

        private void standButton_Click(object sender, RoutedEventArgs e)
        {
            dealerScore.Text = "Dealer score: " + gm.GetDealerScore();
            dealerCardXPos = dealerStartXPos;
            RemoveCards(dealerCards);
            DrawDealer(gm.getDealerCards()[1]);
            DrawDealer(gm.getDealerCards()[0]);

            //TODO pause for 0.5 seconds so that the user can see the dealer getting cards
            while (gm.GetDealerScore() < 17)
            {
                //System.Windows.Forms.Timer myTimer = new Timer();
                //myTimer.Tick += new EventHandler(gm.GiveDealerACard(object obj, EventArgs e));
                gm.GiveDealerACard();
                dealerScore.Text = "Dealer score: " + gm.GetDealerScore();
                DrawDealer(gm.DealerLastCard);
            }

            if (gm.GetDealerScore() > player.GetScore() && gm.GetDealerScore() < 22)
            {
                resultLabel.Text = "Dealer wins!";
            }
            else
            {
                resultLabel.Text = "Player wins!";
            }

        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            resultLabel.Text = "";
            hitMeButton.Enabled = true;
            standButton.Enabled = true;
            RemoveCards(playerCardsToDisplay);
            RemoveCards(dealerCards);

            dealerCardXPos = dealerStartXPos;
            playerCardXPos = startXPos;
            List<Card> playerCards = player.ShowHand();
            gm.StartNewDeal();
            playerScore.Text = "Score: " + player.GetScore();
            dealerScore.Text = "";
            DrawPlayerCard(playerCards[0]);
            DrawPlayerCard(playerCards[1]);
            DrawDealerCardNotShown(gm.DealerVisibleCard);

            if (playerCards[0].Value == playerCards[1].Value)
            {
                splitButton.Enabled = true;
            }
            if (player.GetScore() == 21)
            {
                resultLabel.Text = "Blackjack! Player wins!";
                hitMeButton.Enabled = false;
                standButton.Enabled = false;
            }
        }
        private void RemoveCards(List<PictureBox> cardImages)
        {
            foreach (PictureBox box in cardImages)
            {
                this.Controls.Remove(box);
            }
        }
    }
}
