Script RingOfTheInvincible

Variables
    IsInvincible Is Boolean WithValue (5 + 1) < 2 And True
	RandomVariable Is Number WithValue 0

Run WhenEquipped
	Set IsInvincible To False
	
	If IsInvincible = True Then
		Set RandomVariable To RandomVariable + 1
	Else
		Set RandomVariable To RandomVariable - 1
	EndIf
End

Run WhenInGame
	//This Run block will give the Player Potions Of Health
	=>Message("You will get some potions...")
	If RandomVariable > $Player=>GetAttribute("Health") And $Player=>GetNumberOfItems(PotionOfHealth) < 10 Then
		$PlayerInstance=>AddItem(PotionOfHealth, 2)
		Set RandomVariable To RandomVariable - 1
	EndIf
End