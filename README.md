# warcardgame
The game of war as a .NET Core Console App

- DetermineWinner is the heart of the program. It is a recursive function that is typically called once per round, but can be called recursively when there is a war.
- The Card Pot is where the in-play cards are held until a winner is declared.

## Special Conditions

- I make players shuffle their cards every 26 rounds to prevent a rare condition where the card order causes no one to win. 
- War results in a loss for a player if that player runs out of cards. 
- War can, in extremely rare cases, end up in a draw if both users run out of cards.
