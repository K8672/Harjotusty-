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
using Windows.UI.Xaml.Media.Imaging;
using System.Diagnostics;

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
        private static int startXPos = 0;
        private static int dealerStartXPos = 0;
        private int playerCardXPos = startXPos;
        private int dealerCardXPos = dealerStartXPos;
        //public List<PictureBox> playerCardsToDisplay;
        //public List<PictureBox> dealerCards;

        public GamePage()
        {
            this.InitializeComponent();
            gm = new Manager();
            player = new Player();
            gm.AddPlayer(player);
            //playerCardsToDisplay = new List<PictureBox>();
            //dealerCards = new List<PictureBox>();

            //DrawDealerCardNotShown1.Source = new BitmapImage(new Uri("ms-appx:///Assets/b1fv.png"));

            //splitButton.Hide();
        }


        private void DrawPlayerCard(Card card)
        {
            playerCardXPos += 300;
            Image newCard = new Image();
            newCard.Source = new BitmapImage(new Uri("ms-appx:///Assets/" + card.Value + card.Suit + ".png"));
            newCard.Height = 200;
            //Debug.WriteLine(playerCardXPos);
            //newCard.Margin = new Thickness(playerCardXPos,0,0,0);//new (playerCardXPos, 180)
            //newCard.Name = "newCard";
            //newCard.Size = new System.Drawing.Size(72, 99);
            this.PlayerPanel.Children.Add(newCard);
            //newCard.BringToFront();
            //playerCardsToDisplay.Add(newCard);
            
        }

        private void DrawDealerCardNotShown(Card card)
        {

            dealerCardXPos += 300;
            Image blankCard = new Image();
            blankCard.Source = new BitmapImage(new Uri("ms-appx:///Assets/b1fv.png"));
            blankCard.Height = 200;
            //blankCard.Image = blankImage;
            //blankCard.Margin = new Thickness(dealerCardXPos, 0, 0, 0);//new System.Drawing.Point(dealerCardXPos, 12);
            //blankCard.Name = "newCard";
            //blankCard.Size = new System.Drawing.Size(72, 99);
            this.DealerPanel.Children.Add(blankCard);
            //blankCard.BringToFront();
            //dealerCards.Add(blankCard);

            DrawDealer(card);

        }

        private void DrawDealer(Card card)
        {
            dealerCardXPos += 1;
            Image newCard = new Image();
            newCard.Source = new BitmapImage(new Uri("ms-appx:///Assets/" + card.Value + card.Suit + ".png"));
            newCard.Height = 200;
            //newCard.Image = img;
            
            //newCard.Margin = new Thickness(dealerCardXPos, 0, 0, 0);//new System.Drawing.Point(dealerCardXPos, 12);
            //newCard.Name = "newCard";
            //newCard.Size = new System.Drawing.Size(72, 99);
            this.DealerPanel.Children.Add(newCard);
            //dealerCards.Add(newCard);
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
                    hitMeButton.IsEnabled = false;
                    standButton.IsEnabled = false;
                    
                    resultLabel.Text = "Dealer wins!";
                }
            }
        }

        private void standButton_Click(object sender, RoutedEventArgs e)
        {
            DealerPanel.Children.Clear();
            dealerScore.Text = "Dealer score: " + gm.GetDealerScore();
            dealerCardXPos = dealerStartXPos;
            //RemoveCards(dealerCards);
            //DrawDealerCardNotShown.IsEnabled = false;
            DrawDealer(gm.getDealerCards()[1]);
            DrawDealer(gm.getDealerCards()[0]);
            
            while (gm.GetDealerScore() < 17)
            {               
                gm.GiveDealerACard();
                dealerScore.Text = "Dealer score: " + gm.GetDealerScore();
                DrawDealer(gm.DealerLastCard);
            }

            if (gm.GetDealerScore() > player.GetScore() && gm.GetDealerScore() < 22)
            {
                resultLabel.Text = "Dealer wins!";
                hitMeButton.IsEnabled = false;
                standButton.IsEnabled = false;
            }
            else
            {
                resultLabel.Text = "Player wins!";
                hitMeButton.IsEnabled = false;
                standButton.IsEnabled = false;
            }

        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            DealerPanel.Children.Clear();
            PlayerPanel.Children.Clear();
            resultLabel.Text = "";
            hitMeButton.IsEnabled = true;
            standButton.IsEnabled = true;
            //RemoveCards(playerCardsToDisplay);
            //RemoveCards(dealerCards);

            dealerCardXPos = dealerStartXPos;
            playerCardXPos = startXPos;
            List<Card> playerCards = player.ShowHand();
            gm.StartNewDeal();
            playerScore.Text = "Score: " + player.GetScore();
            dealerScore.Text = "";
            DrawPlayerCard(playerCards[0]);
            DrawPlayerCard(playerCards[1]);
            DrawDealerCardNotShown(gm.DealerVisibleCard);

           /*if (playerCards[0].Value == playerCards[1].Value)
            {
                splitButton.Enabled = true;
            }*/
            if (player.GetScore() == 21)
            {
                resultLabel.Text = "Blackjack! Player wins!";
                hitMeButton.IsEnabled = false;
                standButton.IsEnabled = false;
            }
        }
        
        /*private void RemoveCards(BitmapImage cardImages)
        {
            foreach (BitmapImage box in cardImages)
            {
                this.MyGrid.Children.Remove(box);
            }
        }*/
    }
}
