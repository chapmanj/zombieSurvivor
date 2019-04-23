Public Class frmLvl1
    Private oRoom As New Collection
    Private oABrain As New Collection
    Public oAgent As cAgent
    Public closecellenable As Boolean
    Public pEntityType As Integer
    Public ZombieCount As Integer
    Public ChemCount As Integer
    Public GoalCount As Integer
    Public GOALplaced As Boolean
    Public Filepath As String
    Public OpenedOnce As Boolean
    Public WroteOnce As Boolean
    Public riskfactor As Integer

    'initializing steps----------------------------------------------------------------------------
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim splashcancell As Integer

        splashcancell = MsgBox( _
                "objective:" & vbCrLf & _
                vbCrLf & _
                "design an environment by placing two (2) zombies and/or one (1) chemicalX canister to prevent the agent from reaching the exit/door, which you will also place." & vbCrLf & _
                vbCrLf & _
                "to do this, click the [edit] button. select an entity type to place." & vbCrLf & _
                vbCrLf & _
                "contine with game?", MsgBoxStyle.OkCancel, "Zombie Survivor")

        If splashcancell = 2 Then End

        RoomMaker()
        Agent()
        pEntityType = 0
        oAgent.Position(1, 1) : oAgent.Ammo = 1

    End Sub
    Sub RoomMaker()                     'creates the collection of oCells and adds them to oRoom and enables cell pictures
        Dim oCell As cCell

        For i = 0 To 6
            For j = 0 To 6
                oCell = New cCell : oCell.CellValue(i, j, False, False, False)
                oRoom.Add(oCell, i & j)
            Next
        Next
    End Sub
    Sub Agent()
        'defines the agent as a new object, with a position and ammo for gun function
        oAgent = New cAgent
        oAgent.Position(1, 1)
        oAgent.Ammo = 1

        'DEFINES the Agents brain, originally with no information, but large enough to store 
        'information about the entire room, once reasoned or learned
        Dim C As String : C = "cell"                'used for dynamic image controlling via Me.Controls
        Dim oABCell As cABrainCell                  'Defines the Agentss brain cells (cPCell) to be added to the collection named Brain (oABrain)

        For i = 0 To 6
            For j = 0 To 6
                oABCell = New cABrainCell
                oABCell.CellInfo(i, j, 0, 0, 0, 0, 0, 0)
                oABrain.Add(oABCell, i & j)
            Next
        Next
    End Sub
    '/initializing steps---------------------------------------------------------------------------

    'editing section-------------------------------------------------------------------------------
    Private Sub cmdEDIT_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEDIT.Click
        Dim C As String : C = "cell"
        Dim iCell As Object

        If rZombie.Enabled = False Then
            'enables radio buttons
            cmdClear.Enabled = True
            rZombie.Enabled = True
            rChem.Enabled = True
            rGOAL.Enabled = True
            scrlSafety.Enabled = True
            lblRisk.Enabled = True
            lblSafe.Enabled = True
            lblSave.Enabled = True
            txtSave.Enabled = True
            cmdFind.Enabled = True


            'enables cells for clicking
            For i = 0 To 6
                For j = 0 To 6
                    iCell = C & i & j
                    If 0 < i And i < 6 And 0 < j And j < 6 Then iCell = C & i & j : Me.Controls(iCell).Enabled = True
                Next
            Next

        Else
            'diables radio buttons
            cmdClear.Enabled = False
            rZombie.Enabled = False
            rChem.Enabled = False
            rGOAL.Enabled = False
            scrlSafety.Enabled = False
            lblRisk.Enabled = False
            lblSafe.Enabled = False
            lblSave.Enabled = False
            txtSave.Enabled = False
            cmdFind.Enabled = False

            'disables cells for clicking
            For i = 0 To 6
                For j = 0 To 6
                    iCell = C & i & j
                    If 0 < i And i < 6 And 0 < j And j < 6 Then iCell = C & i & j : Me.Controls(iCell).Enabled = False
                Next
            Next

        End If
    End Sub
    Private Sub cmdClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClear.Click
        Dim C As String : C = "cell"
        Dim oCell As Object
        For i = 1 To 5
            For j = 1 To 5
                oRoom(i & j).CellValue(i, j, False, False, False)
                oRoom(i & j).ZSmell = False : oRoom(i & j).CFumes = False : oRoom(i & j).GDayli = False
                oCell = C & i & j
                Me.Controls(oCell).imagelocation = "@\..\iconset\floor.png"
            Next
        Next
        cell11.ImageLocation = "@\..\iconset\agent.png"
        cell12.ImageLocation = "@\..\iconset\floorD.png"
        cell22.ImageLocation = "@\..\iconset\floorD.png"
        cell21.ImageLocation = "@\..\iconset\floorD.png"
        ZombieCount = 0 : ChemCount = 0 : GoalCount = 0 : GOALplaced = False
    End Sub
    'radio buttons for editing
    Private Sub rZombie_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rZombie.CheckedChanged
        pEntityType = 1
    End Sub
    Private Sub rChem_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rChem.CheckedChanged
        pEntityType = 2
    End Sub
    Private Sub rGOAL_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rGOAL.CheckedChanged
        pEntityType = 3
    End Sub
    Sub EntityPlacement(ByVal A, ByVal B, ByVal Etype)

        Dim C As String : C = "cell"
        Dim oCell As Object

        oCell = C & A & B

        'Defines Up, Down, Left, and Right cell coordinates
        Dim U As Integer : U = B + 1
        Dim D As Integer : D = B - 1
        Dim L As Integer : L = A - 1
        Dim R As Integer : R = A + 1

        If Etype = 1 And ZombieCount < 2 And oRoom(A & B).IsZomb = False Then
            Me.Controls(oCell).imagelocation = "@\..\iconset\zombie.png"
            ZombieCount = ZombieCount + 1
            lblOUT.Text = "you have placed " & ZombieCount & "/2 Zombie(s)."
            oRoom(A & B).cellvalue(A, B, True, False, False)
            'creates Zombie Smell in current and adjacent cells
            oRoom(A & U).cellattributes(True, , )
            oRoom(A & D).cellattributes(True, , )
            oRoom(L & B).cellattributes(True, , )
            oRoom(R & B).cellattributes(True, , )

        ElseIf Etype = 2 And ChemCount < 1 Then
            Me.Controls(oCell).imagelocation = "@\..\iconset\chemx.png"
            ChemCount = ChemCount + 1
            lblOUT.Text = "you have placed " & ChemCount & "/1 ChemicalX containers(s)."
            oRoom(A & B).cellvalue(A, B, False, True, False)
            'creates Hole Breeze in current and adjacent cells
            oRoom(A & U).cellattributes(, True, )
            oRoom(A & D).cellattributes(, True, )
            oRoom(L & B).cellattributes(, True, )
            oRoom(R & B).cellattributes(, True, )

        ElseIf Etype = 3 And GoalCount < 1 Then

            Me.Controls(oCell).imagelocation = "@\..\iconset\door.png"

            oRoom(A & B).cellvalue(A, B, False, False, True)
            oRoom(A & B).cellattributes(, , True)

            GoalCount = GoalCount + 1
            lblOUT.Text = "you have placed " & GoalCount & "/1 Goal(s)."

            GOALplaced = True

        ElseIf Etype = 0 Then
            lblOUT.Text = "place an entity."
        Else : MsgBox("you have reached entity limit for this level.")
        End If
    End Sub
    Private Sub cmdFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFind.Click
        FolderBrowserDialog1.ShowDialog()
        Filepath = FolderBrowserDialog1.SelectedPath
        txtSave.Text = Filepath
    End Sub
    '/editing section------------------------------------------------------------------------------

    'Agent Stuff-----------------------------------------------------------------------------------
    Private Sub cmdAgentRun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAgentRun.Click
        runwalk(1)
    End Sub
    Private Sub cmdAgentStep_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAgentStep.Click
        runwalk(0)
    End Sub
    Sub runwalk(ByVal rw)
        If GOALplaced = True Then
            If txtSave.Text <> "" Then
                riskfactor = scrlSafety.Value / 100
                Filepath = txtSave.Text
                If closecellenable = False Then
                    cell12.ImageLocation = "@\..\iconset\floor.png"
                    cell22.ImageLocation = "@\..\iconset\floor.png"
                    cell21.ImageLocation = "@\..\iconset\floor.png"
                    closecellenable = True
                End If
                If rw = 0 Then
                    AgentMovement()
                Else
                    For i = 0 To 500                    'loops until goal state is found or steps are up
                        AgentMovement()
                    Next

                    MsgBox("agent did not find the goal in alloted number of steps.")
                End If

            Else : MsgBox("enter a filepath to save room and agent information.")
            End If
        ElseIf rZombie.Enabled = True Then : MsgBox("to finish editing, press the [Edit} button again.")
        Else : MsgBox("you must place a DOOR for the simulation to start.")
        End If
    End Sub

    Private Sub AgentMovement()

        cmdEDIT.Enabled = False
        cmdClear.Enabled = False
        rZombie.Enabled = False
        rChem.Enabled = False
        rGOAL.Enabled = False

        'WRITES user designed room to a text file
        WriteRoom()

        'DEFINE Agent position
        Dim A As Integer : A = oAgent.X
        Dim B As Integer : B = oAgent.Y
        'Potential Movemnts Up (U), Down (D), Left (L), & Right (R)
        Dim U As Integer : U = B + 1
        Dim D As Integer : D = B - 1
        Dim L As Integer : L = A - 1
        Dim R As Integer : R = A + 1


        'TELL, REASON, AND RECORD--------------------------------------------------------------
        'DEFINITE existance of entities------                     
        Dim Zo As Integer : If oRoom(A & B).IsZomb = True Then Zo = 3 : oABrain(A & B).pZomb = 3
        'If Agent is in the same cell as a zombie
        If Zo = 3 Then
            oAgent.UseGun(1)
            oRoom(A & B).IsZomb = False
            oRoom(A & U).Zsmell = False
            oRoom(A & D).Zsmell = False
            oRoom(L & B).zsmell = False
            oRoom(R & B).zSmell = False
        End If
        'If Agent walks into a cell with a chemicalX canister
        Dim cX As Integer : If oRoom(A & B).IsChem = True Then cX = 3 : oABrain(A & B).pChem = 3
        If cX = 3 Then
            PrintToText(A, B, oABrain(A & B).pZomb, oABrain(A & B).pChem, oABrain(A & B).pGoal, oABrain(A & B).pSmel, oABrain(A & B).pFume, oABrain(A & B).pDayl, "      MOVE")
            MsgBox("Agent: the chemicalX canister made me a zombie ... game over.")
            End
        End If
        'If Agent walks into a cell with GOAL
        Dim Go As Integer : If oRoom(A & B).IsGoal = True Then Go = 3 : oABrain(A & B).pGoal = 3

        '/DEFINITE existance of entities-----

        'DEFINITE existance of attributes
        Dim Sm As Integer : If oRoom(A & B).ZSmell = True Then Sm = 3 : oABrain(A & B).pSmel = 3
        Dim Fu As Integer : If oRoom(A & B).CFumes = True Then Fu = 3 : oABrain(A & B).pFume = 3
        Dim Dl As Integer : If oRoom(A & B).GDayli = True Then Dl = 3 : oABrain(A & B).pDayl = 3

        'GOAL check: if Agent has reached the GOAL then he wins and Ends execution
        If Dl = 3 Then MsgBox("Agent: I escaped the zombies.") : oABrain(A & B).pGoal = 3 : oABrain(A & B).pDayl = 3 : End

        'WRITES current position, potential existance of any Entities and any Attributes
        PrintToText("X", "Y", "PZomb", "PHole", "PGoal", "PSmel", "PBrez", "PDayl", "LOGIC/MOVE")
        PrintToText(A, B, oABrain(A & B).pZomb, oABrain(A & B).pChem, oABrain(A & B).pGoal, oABrain(A & B).pSmel, oABrain(A & B).pfume, oABrain(A & B).pDayl, "      MOVE")
        PrintToText("-", "-", "-", "-", "-", "-", "-", "-", "-")

        'REASONING---------------------------
        'if it smells here then maybe there is a zombie near, else there is not
        If Sm = 3 Then
            oABrain(A & U).cellinfo(A, U, 1, 0, 0, 0, 0, 0) : PrintToText(A, U, oABrain(A & U).pZomb, oABrain(A & U).pChem, oABrain(A & U).pGoal, oABrain(A & U).pSmel, oABrain(A & U).pFume, oABrain(A & U).pDayl, "LOGIC")
            oABrain(A & D).cellinfo(A, D, 1, 0, 0, 0, 0, 0) : PrintToText(A, D, oABrain(A & D).pZomb, oABrain(A & D).pChem, oABrain(A & D).pGoal, oABrain(A & D).pSmel, oABrain(A & D).pFume, oABrain(A & D).pDayl, "LOGIC")
            oABrain(L & B).cellinfo(L, B, 1, 0, 0, 0, 0, 0) : PrintToText(L, B, oABrain(L & B).pZomb, oABrain(L & B).pChem, oABrain(L & B).pGoal, oABrain(L & B).pSmel, oABrain(L & B).pFume, oABrain(L & B).pDayl, "LOGIC")
            oABrain(R & B).cellinfo(R, B, 1, 0, 0, 0, 0, 0) : PrintToText(R, B, oABrain(R & B).pZomb, oABrain(R & B).pChem, oABrain(R & B).pGoal, oABrain(R & B).pSmel, oABrain(R & B).pFume, oABrain(R & B).pDayl, "LOGIC")
            PrintToText("-", "-", "-", "-", "-", "-", "-", "-", "-")
        Else
            oABrain(A & U).cellinfo(A, U, 2, 0, 0, 0, 0, 0) : PrintToText(A, U, oABrain(A & U).pZomb, oABrain(A & U).pChem, oABrain(A & U).pGoal, oABrain(A & U).pSmel, oABrain(A & U).pFume, oABrain(A & U).pDayl, "LOGIC")
            oABrain(A & D).cellinfo(A, D, 2, 0, 0, 0, 0, 0) : PrintToText(A, D, oABrain(A & D).pZomb, oABrain(A & D).pChem, oABrain(A & D).pGoal, oABrain(A & D).pSmel, oABrain(A & D).pFume, oABrain(A & D).pDayl, "LOGIC")
            oABrain(L & B).cellinfo(L, B, 2, 0, 0, 0, 0, 0) : PrintToText(L, B, oABrain(L & B).pZomb, oABrain(L & B).pChem, oABrain(L & B).pGoal, oABrain(L & B).pSmel, oABrain(L & B).pFume, oABrain(L & B).pDayl, "LOGIC")
            oABrain(R & B).cellinfo(R, B, 2, 0, 0, 0, 0, 0) : PrintToText(R, B, oABrain(R & B).pZomb, oABrain(R & B).pChem, oABrain(R & B).pGoal, oABrain(R & B).pSmel, oABrain(R & B).pFume, oABrain(R & B).pDayl, "LOGIC")
            PrintToText("-", "-", "-", "-", "-", "-", "-", "-", "-")
        End If

        'if it breezes here then maybe there is a hole near if not, there is not
        If Fu = 3 Then
            oABrain(A & U).cellinfo(A, U, 0, 1, 0, 0, 0, 0) : PrintToText(A, U, oABrain(A & U).pZomb, oABrain(A & U).pChem, oABrain(A & U).pGoal, oABrain(A & U).pSmel, oABrain(A & U).pFume, oABrain(A & U).pDayl, "LOGIC")
            oABrain(A & D).cellinfo(A, D, 0, 1, 0, 0, 0, 0) : PrintToText(A, D, oABrain(A & D).pZomb, oABrain(A & D).pChem, oABrain(A & D).pGoal, oABrain(A & D).pSmel, oABrain(A & D).pFume, oABrain(A & D).pDayl, "LOGIC")
            oABrain(L & B).cellinfo(L, B, 0, 1, 0, 0, 0, 0) : PrintToText(L, B, oABrain(L & B).pZomb, oABrain(L & B).pChem, oABrain(L & B).pGoal, oABrain(L & B).pSmel, oABrain(L & B).pFume, oABrain(L & B).pDayl, "LOGIC")
            oABrain(R & B).cellinfo(R, B, 0, 1, 0, 0, 0, 0) : PrintToText(R, B, oABrain(R & B).pZomb, oABrain(R & B).pChem, oABrain(R & B).pGoal, oABrain(R & B).pSmel, oABrain(R & B).pFume, oABrain(R & B).pDayl, "LOGIC")
            PrintToText("-", "-", "-", "-", "-", "-", "-", "-", "-")
        Else
            oABrain(A & U).cellinfo(A, U, 0, 2, 0, 0, 0, 0) : PrintToText(A, U, oABrain(A & U).pZomb, oABrain(A & U).pChem, oABrain(A & U).pGoal, oABrain(A & U).pSmel, oABrain(A & U).pFume, oABrain(A & U).pDayl, "LOGIC")
            oABrain(A & D).cellinfo(A, D, 0, 2, 0, 0, 0, 0) : PrintToText(A, D, oABrain(A & D).pZomb, oABrain(A & D).pChem, oABrain(A & D).pGoal, oABrain(A & D).pSmel, oABrain(A & D).pFume, oABrain(A & D).pDayl, "LOGIC")
            oABrain(L & B).cellinfo(L, B, 0, 2, 0, 0, 0, 0) : PrintToText(L, B, oABrain(L & B).pZomb, oABrain(L & B).pChem, oABrain(L & B).pGoal, oABrain(L & B).pSmel, oABrain(L & B).pFume, oABrain(L & B).pDayl, "LOGIC")
            oABrain(R & B).cellinfo(R, B, 0, 2, 0, 0, 0, 0) : PrintToText(R, B, oABrain(R & B).pZomb, oABrain(R & B).pChem, oABrain(R & B).pGoal, oABrain(R & B).pSmel, oABrain(R & B).pFume, oABrain(R & B).pDayl, "LOGIC")
            PrintToText("-", "-", "-", "-", "-", "-", "-", "-", "-")
        End If

        'if the Agent believes there may be a Zombie in a cell and any two adjacent squares smell then
        'say that there is a zombie in that cell

        Dim Smcount As Integer : Smcount = 0
        Dim NScount As Integer : NScount = 0
        Dim cXcount As Integer : cXcount = 0
        Dim NHcount As Integer : NHcount = 0

        For i = 1 To 5
            For j = 1 To 5
                If oABrain(i & j).Pzomb = 1 Then

                    If oABrain(i & j + 1).Psmel = 3 Then Smcount = Smcount + 1
                    If oABrain(i & j - 1).Psmel = 3 Then Smcount = Smcount + 1
                    If oABrain(i - 1 & j).Psmel = 3 Then Smcount = Smcount + 1
                    If oABrain(i + 1 & j).Psmel = 3 Then Smcount = Smcount + 1

                    If Smcount >= 2 Then
                        oABrain(i & j).PZomb = 3
                        MsgBox("Agent: I reason that there is a zombie in cell (" & i & "," & j & ")")
                    End If
                    Smcount = 0
                End If
            Next
        Next

        'if the Agent believes there may be a ChemicalX canister in a cell and any two adjacent squares have fumes then
        'say that there is a Hole in that cell

        For m = 1 To 5
            For n = 1 To 5
                If oABrain(m & n).Pchem = 1 Then

                    If oABrain(m & n + 1).Pfume = 3 Then cXcount = cXcount + 1
                    If oABrain(m & n - 1).Pfume = 3 Then cXcount = cXcount + 1
                    If oABrain(m - 1 & n).Pfume = 3 Then cXcount = cXcount + 1
                    If oABrain(m + 1 & n).Pfume = 3 Then cXcount = cXcount + 1

                    If cXcount >= 2 Then
                        oABrain(m & n).Pchem = 3
                        MsgBox("Agent: I reason that there is a chemicalX canister in cell (" & m & "," & n & ")")
                    End If
                    cXcount = 0
                End If
            Next
        Next

        Dim C As String : C = "cell"
        Dim oCell As Object
1:      Dim dir As Integer : dir = Math.Floor(4 * Rnd())    'random direction generator
        '/TELL, REASON, AND RECORD-------------------------------------------------------------


        'AGENT MOVEMENT SECTION----------------------------------------------------------------
        If dir = 0 Then
            If U < 6 Then 'as long as up is within bounds

                If (oABrain(A & U).pZomb = 2 And oABrain(A & U).pChem = 2) Then
                    'move from A&B to A&U
                    oCell = C & A & B : Me.Controls(oCell).imagelocation = "@\..\iconset\floor.png"
                    oAgent.Position(A, U)   'records new position
                    oCell = C & A & U : Me.Controls(oCell).imagelocation = "@\..\iconset\agent.png"

                ElseIf oABrain(A & U).pZomb <= 1 And oABrain(A & U).pChem <= 1 And riskfactor < Rnd() Then
                    'risk factor (to avoid getting stuck)
                    'move from A&B to A&U
                    oCell = C & A & B : Me.Controls(oCell).imagelocation = "@\..\iconset\floor.png"
                    oAgent.Position(A, U)   'records new position
                    oCell = C & A & U : Me.Controls(oCell).imagelocation = "@\..\iconset\agent.png"

                ElseIf (oABrain(A & U).pZomb = 3 And oAgent.Ammo > 0 And 1 - riskfactor < Rnd()) Then
                    'risk factor (to try to kill a zombie)
                    'move from A&B to A&U
                    oCell = C & A & B : Me.Controls(oCell).imagelocation = "@\..\iconset\floor.png"
                    oAgent.Position(A, U)   'records new position
                    oCell = C & A & U : Me.Controls(oCell).imagelocation = "@\..\iconset\agent.png"
                Else
                    GoTo 1
                End If
            Else
                GoTo 1
            End If

        ElseIf dir = 1 Then
            If 0 < D Then 'as long as up is within bounds

                If (oABrain(A & D).pzomb = 2 And oABrain(A & D).pChem = 2) Then
                    'move from A&B to A&U
                    oCell = C & A & B : Me.Controls(oCell).imagelocation = "@\..\iconset\floor.png"
                    oAgent.Position(A, D)   'records new position
                    oCell = C & A & D : Me.Controls(oCell).imagelocation = "@\..\iconset\agent.png"

                ElseIf oABrain(A & D).pzomb <= 1 And oABrain(A & U).pChem <= 1 And riskfactor < Rnd() Then
                    'risk factor (to avoid getting stuck)
                    'move from A&B to A&U
                    oCell = C & A & B : Me.Controls(oCell).imagelocation = "@\..\iconset\floor.png"
                    oAgent.Position(A, D)   'records new position
                    oCell = C & A & D : Me.Controls(oCell).imagelocation = "@\..\iconset\agent.png"

                ElseIf (oABrain(A & D).pZomb = 3 And oAgent.Ammo > 0 And 1 - riskfactor < Rnd()) Then
                    'risk factor (to try to kill a zombie)
                    'move from A&B to A&U
                    oCell = C & A & B : Me.Controls(oCell).imagelocation = "@\..\iconset\floor.png"
                    oAgent.Position(A, D)   'records new position
                    oCell = C & A & D : Me.Controls(oCell).imagelocation = "@\..\iconset\agent.png"
                Else
                    GoTo 1
                End If
            Else
                GoTo 1
            End If

        ElseIf dir = 2 Then
            If 0 < L Then 'as long as up is within bounds

                If (oABrain(L & B).pzomb = 2 And oABrain(L & B).pChem = 2) Then
                    'move from A&B to L&B
                    oCell = C & A & B : Me.Controls(oCell).imagelocation = "@\..\iconset\floor.png"
                    oAgent.Position(L, B)   'records new(position)
                    oCell = C & L & B : Me.Controls(oCell).imagelocation = "@\..\iconset\agent.png"

                ElseIf oABrain(L & B).pZomb <= 1 And oABrain(L & U).pChem <= 1 And riskfactor < Rnd() Then
                    'risk factor (to avoid getting stuck)
                    oCell = C & A & B : Me.Controls(oCell).imagelocation = "@\..\iconset\floor.png"
                    oAgent.Position(L, B)   'records new(position)
                    oCell = C & L & B : Me.Controls(oCell).imagelocation = "@\..\iconset\agent.png"

                ElseIf (oABrain(L & B).Pzomb = 3 And oAgent.Ammo > 0 And 1 - riskfactor < Rnd()) Then
                    'risk factor (to try to kill a zombie)
                    'move from A&B to A&U
                    oCell = C & A & B : Me.Controls(oCell).imagelocation = "@\..\iconset\floor.png"
                    oAgent.Position(L, B)   'records new(position)
                    oCell = C & L & B : Me.Controls(oCell).imagelocation = "@\..\iconset\agent.png"
                Else
                    GoTo 1
                End If
            Else
                GoTo 1
            End If

        ElseIf dir = 3 Then

            If R < 6 Then 'as long as up is within bounds

                If (oABrain(R & B).pzomb = 2 And oABrain(R & B).pChem = 2) Then
                    'move from A&B to A&U
                    oCell = C & A & B : Me.Controls(oCell).imagelocation = "@\..\iconset\floor.png"
                    oAgent.Position(R, B)   'records new position
                    oCell = C & R & B : Me.Controls(oCell).imagelocation = "@\..\iconset\agent.png"

                ElseIf oABrain(R & B).pZomb <= 1 And oABrain(R & B).pChem <= 1 And riskfactor < Rnd() Then
                    'risk factor (to avoid getting stuck)
                    oCell = C & A & B : Me.Controls(oCell).imagelocation = "@\..\iconset\floor.png"
                    oAgent.Position(A, U)   'records new position
                    oCell = C & A & U : Me.Controls(oCell).imagelocation = "@\..\iconset\agent.png"

                ElseIf (oABrain(R & B).PZomb = 3 And oAgent.Ammo > 0 And 1 - riskfactor < Rnd()) Then
                    'risk factor (to try to kill a zombie)
                    'move from A&B to A&U
                    oCell = C & A & B : Me.Controls(oCell).imagelocation = "@\..\iconset\floor.png"
                    oAgent.Position(R, B)   'records new position
                    oCell = C & R & B : Me.Controls(oCell).imagelocation = "@\..\iconset\agent.png"
                Else
                    GoTo 1
                End If
            Else
                GoTo 1
            End If

        End If
        '/AGENT MOVEMENT SECTION---------------------------------------------------------------

    End Sub
    '/Agent Stuff----------------------------------------------------------------------------------

    'ON CLICK cell commands------------------------------------------------------------------------
    Private Sub cell11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cell11.Click
        lblOUT.Text = ("you cannot place an entity here.") 'EntityPlacement(1, 1, pEntityType)
    End Sub
    Private Sub cell21_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cell21.Click
        lblOUT.Text = ("you cannot place an entity here.")  'EntityPlacement(2, 1, pEntityType)
    End Sub
    Private Sub cell31_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cell31.Click
        EntityPlacement(3, 1, pEntityType)
    End Sub
    Private Sub cell41_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cell41.Click
        EntityPlacement(4, 1, pEntityType)
    End Sub
    Private Sub cell51_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cell51.Click
        EntityPlacement(5, 1, pEntityType)
    End Sub

    Private Sub cell12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cell12.Click
        lblOUT.Text = ("you cannot place an entity here.") 'EntityPlacement(1, 2, pEntityType)
    End Sub
    Private Sub cell22_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cell22.Click
        lblOUT.Text = ("you cannot place an entity here.") 'EntityPlacement(2, 2, pEntityType)
    End Sub
    Private Sub cell32_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cell32.Click
        EntityPlacement(3, 2, pEntityType)
    End Sub
    Private Sub cell42_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cell42.Click
        EntityPlacement(4, 2, pEntityType)
    End Sub
    Private Sub cell52_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cell52.Click
        EntityPlacement(5, 2, pEntityType)
    End Sub

    Private Sub cell13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cell13.Click
        EntityPlacement(1, 3, pEntityType)
    End Sub
    Private Sub cell23_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cell23.Click
        EntityPlacement(2, 3, pEntityType)
    End Sub
    Private Sub cell33_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cell33.Click
        EntityPlacement(3, 3, pEntityType)
    End Sub
    Private Sub cell43_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cell43.Click
        EntityPlacement(4, 3, pEntityType)
    End Sub
    Private Sub cell53_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cell53.Click
        EntityPlacement(5, 3, pEntityType)
    End Sub

    Private Sub cell14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cell14.Click
        EntityPlacement(1, 4, pEntityType)
    End Sub
    Private Sub cell24_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cell24.Click
        EntityPlacement(2, 4, pEntityType)
    End Sub
    Private Sub cell34_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cell34.Click
        EntityPlacement(3, 4, pEntityType)
    End Sub
    Private Sub cell44_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cell44.Click
        EntityPlacement(4, 4, pEntityType)
    End Sub
    Private Sub cell54_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cell54.Click
        EntityPlacement(5, 4, pEntityType)
    End Sub

    Private Sub cell15_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cell15.Click
        EntityPlacement(1, 5, pEntityType)
    End Sub
    Private Sub cell25_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cell25.Click
        EntityPlacement(2, 5, pEntityType)
    End Sub
    Private Sub cell35_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cell35.Click
        EntityPlacement(3, 5, pEntityType)
    End Sub
    Private Sub cell45_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cell45.Click
        EntityPlacement(4, 5, pEntityType)
    End Sub
    Private Sub cell55_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cell55.Click
        EntityPlacement(5, 5, pEntityType)
    End Sub
    '/ON CLICK cell commands-----------------------------------------------------------------------

    'WRITE to files--------------------------------------------------------------------------------
    Private Sub WriteRoom()
        If WroteOnce = False Then
            FileOpen(1, Filepath & "\RoomInfo.txt", OpenMode.Output) ' Open file for output.
            Print(1, "XY", TAB(4), "IsZomb", TAB(12), "IsChem", TAB(20), "IsGoal", TAB(28), "ZSmell", TAB(36), "CFumes", TAB(44), "GDayli", TAB(0))

            For i = 1 To 5
                For j = 1 To 5
                    Print(1, i & j, TAB(4), oRoom(i & j).isZomb, TAB(12), oRoom(i & j).IsChem, TAB(20), oRoom(i & j).IsGoal, TAB(28), oRoom(i & j).ZSmell, TAB(36), oRoom(i & j).CFumes, TAB(44), oRoom(i & j).GDayli, TAB(0))
                Next
            Next
            FileClose(1)
            WroteOnce = True
        End If
    End Sub
    Private Sub PrintToText(ByVal a, ByVal b, ByVal z, ByVal c, ByVal g, ByVal s, ByVal e, ByVal d, Optional ByVal logic = "")
        If OpenedOnce = False Then
            FileOpen(1, Filepath & "\AgentBrain.txt", OpenMode.Output) ' Open file for output.
            Print(1, "XY", TAB(4), "PZomb", TAB(12), "PChem", TAB(20), "PGoal", TAB(28), "PSmel", TAB(36), "PFume", TAB(44), "PDayl", TAB(52), "LOGIC/MOVE", TAB(0))
            OpenedOnce = True
        Else
            Print(1, a & b, TAB(4), z, TAB(12), c, TAB(20), g, TAB(28), s, TAB(36), e, TAB(44), d, TAB(52), logic, TAB(0))
        End If
    End Sub
    '/WRITE to files-------------------------------------------------------------------------------
End Class

