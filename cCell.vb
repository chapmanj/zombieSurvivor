Public Class cCell
    'defins properties of the cell.
    'gives the x and y coordinate and the tags of player, human, zombie of goal
    Public X As Integer
    Public Y As Integer

    'Public IsPlay As Boolean
    Public IsChem As Boolean
    Public IsZomb As Boolean
    Public IsGOAL As Boolean

    Public ZSmell As Boolean
    Public CFumes As Boolean
    Public GDayli As Boolean

    Public Sub CellValue(ByVal lx, ByVal ly, ByVal lIsZomb, ByVal lIsChem, Optional ByVal lIsGoal = False)
        'ByVal lIsPlay, 
        X = lx
        Y = ly
        'IsPlay = lIsPlay
        IsChem = lIsChem
        IsZomb = lIsZomb
        IsGOAL = lIsGoal
    End Sub

    Public Sub CellAttributes(Optional ByVal lZSmell = False, Optional ByVal lCFumes = False, Optional ByVal lGDayli = False)
        If (ZSmell = False And lZSmell = True) Then ZSmell = lZSmell
        If (CFumes = False And lCFumes = True) Then CFumes = lCFumes
        If (GDayli = False And lGDayli = True) Then GDayli = lGDayli
    End Sub

End Class
