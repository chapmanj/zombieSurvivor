Public Class cABrainCell
    Public CellA As Integer
    Public CellB As Integer
    Public Pzomb As Integer
    Public Pchem As Integer
    Public Pgoal As Integer
    Public Psmel As Integer
    Public Pfume As Integer
    Public Pdayl As Integer

    Public Sub CellInfo(ByVal a, ByVal b, ByVal lpzomb, ByVal lpchem, ByVal lpgoal, ByVal lpsmel, ByVal lpfume, ByVal lpdayl)
        '0 = no info
        '1 = maybe
        '2 = NO
        '3 = YES

        CellA = a
        CellB = b
        If Pzomb < lpzomb Then Pzomb = lpzomb
        If Pchem < lpchem Then Pchem = lpchem
        If Pgoal < lpgoal Then Pgoal = lpgoal
        If Psmel < lpsmel Then Psmel = lpsmel
        If Pfume < lpfume Then Pfume = lpfume
        If Pdayl < lpdayl Then Pdayl = lpdayl

    End Sub
End Class
