Public Class cAgent
    Public X As Integer
    Public Y As Integer
    Public Ammo As Integer

    Public Sub Position(ByVal lx, ByVal ly)           'players position
        X = lx
        Y = ly
    End Sub

    Public Sub UseGun(ByVal lAmmo)

        If Ammo = 1 And lAmmo = 1 Then
            Ammo = Ammo - 1
            MsgBox("Agent: i killed the zombie." & Ammo)
            'returns to main control
        Else
            MsgBox("Agent: i am out of ammo." & vbCrLf & "        zombies ate my brains.")
            End
        End If
    End Sub


End Class