Script RingOfExampleScript

Variables
	FirstEquipped Is Boolean WithValue False
	
Run WhenEquipped
	If FirstEquipped = False Then
		Set FirstEquipped To True
		=>Message("You feel something strange...")
		ExampleQuest=>Set("State", 1)
	EndIf
End