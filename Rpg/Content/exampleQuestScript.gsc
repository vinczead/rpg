Script ExampleQuestScript

Variables
	Npc Is Reference
	PlayerRoom Is Reference

Run WhenInGame
	If ExampleQuest=>Get("State") = 0
		Return
	
	//Ha a jatekos belepett felvette a gyurut, es belepett a szobaba,
	//akkor letrehozunk egy NPC-t
	
	Set PlayerRoom To Player.Get("Room")
	If ExampleQuest=>Get("State") = 1 And PlayerRoom = OtherRoom Then
		=>Message("The floor trembles under your steps!")
		ExampleQuest=>Set("State", 2)
		ExampleNPCB=>Spawn(OtherRoom, 200, 200, "ExampleNPC")
	EndIf
End