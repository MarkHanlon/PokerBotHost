using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PokerBotHost.Common
{
    /// <summary>
    /// Define the available suits
    /// </summary>
    public enum Suit
    {
        Clubs = 0,
        Diamonds,
        Hearts,
        Spades
    }

    /// <summary>
    /// Define the card values
    /// </summary>
    public enum Value
    {
        Two = 2,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace
    }

    public class Card
    {

        private Suit _suit;
        private Value _value;

        public Card(Suit suit, Value value)
        {
            _suit = suit;
            _value = value;
        }
       
        public override string ToString()
        {
            char suit, value;

            switch (_suit)
            {
                case Suit.Clubs:    { suit = 'C'; break; }
                case Suit.Diamonds: { suit = 'D'; break; }
                case Suit.Hearts:   { suit = 'H'; break; }
                case Suit.Spades:   { suit = 'S'; break; }
                default:            { suit = '?'; break; }
            }

            switch (_value)
            {
                case Value.Ace:   { value = 'A'; break; }
                case Value.Two:   { value = '2'; break; }
                case Value.Three: { value = '3'; break; }
                case Value.Four:  { value = '4'; break; }
                case Value.Five:  { value = '5'; break; }
                case Value.Six:   { value = '6'; break; }
                case Value.Seven: { value = '7'; break; }
                case Value.Eight: { value = '8'; break; }
                case Value.Nine:  { value = '9'; break; }
                case Value.Ten:   { value = 'T'; break; }
                case Value.Jack:  { value = 'J'; break; }
                case Value.Queen: { value = 'Q'; break; }
                case Value.King:  { value = 'K'; break; }
                default:          { value = '?'; break; }
            }

            return String.Format("{0}{1}", value, suit);
        }
    }
}
