Script EsxampleNpcScript

Run WhenKilled
	If Attacker = Player Then 
		ExampleQuest=>Set("State", 100)
	EndIf
End