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
    public partial class GamePage : Page
    {
        //pelin koodi
        public Player player;
        public Manager gm;
        private static int startXPos = 0;
        private static int dealerStartXPos = 0;
        private int playerCardXPos = startXPos;
        private int dealerCardXPos = dealerStartXPos;

        public GamePage()
        {
            this.InitializeComponent();
            gm = new Manager();
            player = new Player();
            gm.AddPlayer(player);
            hitMeButton.IsEnabled = false;
            standButton.IsEnabled = false;
        }

        //Pelaaja nostaa kortin
        private void DrawPlayerCard(Card card)
        {
            Image newCard = new Image();
            newCard.Source = new BitmapImage(new Uri("ms-appx:///Assets/" + card.Value + card.Suit + ".png"));
            newCard.Height = 200;
            this.PlayerPanel.Children.Add(newCard);
        }
        //jakajan piilotettu kortti
        private void DrawDealerCardNotShown(Card card)
        {
            Image blankCard = new Image();
            blankCard.Source = new BitmapImage(new Uri("ms-appx:///Assets/b1fv.png"));
            blankCard.Height = 200;            
            this.DealerPanel.Children.Add(blankCard);

            DrawDealer(card);

        }
        //jakajan näkyvät kortit
        private void DrawDealer(Card card)
        {           
            Image newCard = new Image();
            newCard.Source = new BitmapImage(new Uri("ms-appx:///Assets/" + card.Value + card.Suit + ".png"));
            newCard.Height = 200;
            this.DealerPanel.Children.Add(newCard);
        }
        //palautuu takaisin päävalikkoon
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
        //jakaa kortin pelaajalle
        private void hitMeButton_Click(object sender, RoutedEventArgs e)
        {
            if (!player.Busted)
            {
                player.HitMe(gm.DealCard());
                playerScore.Text = "Score: " + player.GetScore();
                DrawPlayerCard(player.LastCard());
                //laittaa hit ja stand nappulat pois käytöstä jos häviää
                if (player.Busted)
                {
                    hitMeButton.IsEnabled = false;
                    standButton.IsEnabled = false;
                    
                    resultLabel.Text = "Dealer wins!";
                }
            }
        }
        //Lopettaa korttien noston
        private void standButton_Click(object sender, RoutedEventArgs e)
        {
            //tyhjentää jakajan stackpaneelin, että siihen ei jää korttia väärinpäin.
            DealerPanel.Children.Clear();
            dealerScore.Text = "Dealer score: " + gm.GetDealerScore();
            //dealerCardXPos = dealerStartXPos;
            DrawDealer(gm.getDealerCards()[1]);
            DrawDealer(gm.getDealerCards()[0]);
            
            //jakaja joutuu nostamaan kortteja 17 asti
            while (gm.GetDealerScore() < 17)
            {               
                gm.GiveDealerACard();
                dealerScore.Text = "Dealer score: " + gm.GetDealerScore();
                DrawDealer(gm.DealerLastCard);
            }

            //jos jakajan score > pelaajan ja alle 22 jakaja voittaa. Samalla poistaa hit ja stand buttonin käytöstä.
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
        //aloittaa pelin
        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            //Tyhjentää molemmat stackpaneelit ennen pelin alkua.
            DealerPanel.Children.Clear();
            PlayerPanel.Children.Clear();
            resultLabel.Text = "";
            //aktivoi hit ja stand buttonit
            hitMeButton.IsEnabled = true;
            standButton.IsEnabled = true;

            //dealerCardXPos = dealerStartXPos;
            //playerCardXPos = startXPos;
            List<Card> playerCards = player.ShowHand();
            gm.StartNewDeal();
            playerScore.Text = "Score: " + player.GetScore();
            dealerScore.Text = "";
            DrawPlayerCard(playerCards[0]);
            DrawPlayerCard(playerCards[1]);
            DrawDealerCardNotShown(gm.DealerVisibleCard);

            //Blackjack
            if (player.GetScore() == 21)
            {
                resultLabel.Text = "Blackjack! Player wins!";
                hitMeButton.IsEnabled = false;
                standButton.IsEnabled = false;
            }
        }
        
     
    }
}
