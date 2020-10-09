// Setup the main game logic.

(function () {
    var prefixEl = document.querySelector('#prefix');
    var primaryTextEl = document.querySelector('.primary');
    var secondaryTextEl = document.querySelector('.secondary');
    var currentPlayerNameEl = document.querySelector('#current-player');
    var otherPlayerNameEl = document.querySelector('#other-player');
    var playAgainEl = document.querySelector('#play-again');
    var playAgainBtnEl = document.querySelector('#play-again-btn');
    var gameBoardEl = document.querySelector('#board');

    playAgainBtnEl.addEventListener('click', () => location.reload());
    gameBoardEl.addEventListener('click', placeGamePiece);
    currentPlayerNameEl.addEventListener("keydown", Game.do.handleNameChange);
    otherPlayerNameEl.addEventListener("keydown", Game.do.handleNameChange);
    $('#selectAI').change(selectAI);

    var aiPlayer = 'Random Forest';

    function selectAI() {
        aiPlayer = $('#selectAI option:selected').val();
    }
    

    function placeGamePiece(e) {
        if (e.target.tagName !== 'BUTTON') return;

        var targetCell = e.target.parentElement;
        var targetRow = targetCell.parentElement;
        var targetRowCells = [...targetRow.children];
        var gameBoardRowsEls = [...document.querySelectorAll('#board tr')];

        // Detect the x and y position of the button clicked.
        var y_pos = gameBoardRowsEls.indexOf(targetRow);
        var x_pos = targetRowCells.indexOf(targetCell);
        placeGamePieceByPosition(x_pos, y_pos);

    };

    function placeGamePieceByPosition(x_pos, y_pos) {
        // Ensure the piece falls to the bottom of the column.
        y_pos = Game.do.dropToBottom(x_pos, y_pos);

        if (Game.check.isPositionTaken(x_pos, y_pos)) {
            alert(Game.config.takenMsg);
            return;
        }

        // Add the piece to the board.
        Game.do.addDiscToBoard(x_pos, y_pos);
        Game.do.printBoard();
        var gameBoardEl = document.querySelector('#board');
        // Check to see if we have a winner.
        if (Game.check.isVerticalWin() || Game.check.isHorizontalWin() || Game.check.isDiagonalWin()) {
            gameBoardEl.removeEventListener('click', placeGamePiece);
            prefixEl.textContent = Game.config.winMsg;
            currentPlayerNameEl.contentEditable = false;
            secondaryTextEl.remove();
            playAgainEl.classList.add('show');
            return;
        } else if (Game.check.isGameADraw()) {
            gameBoardEl.removeEventListener('click', placeGamePiece);
            primaryTextEl.textContent = Game.config.drawMsg;
            secondaryTextEl.remove();
            playAgainEl.classList.add('show');
            return;
        }

        // Change player.
        Game.do.changePlayer();

        var gameBoardEl = document.querySelector('#board');
        if (Game.currentPlayer == 'red') {
            gameBoardEl.removeEventListener('click', placeGamePiece);
            Game.do.playAI(aiPlayer).then(function (scores) {
                var score1 = scores["ProbabilityColumn1"];
                var score2 = scores["ProbabilityColumn2"];
                var score3 = scores["ProbabilityColumn3"];
                var score4 = scores["ProbabilityColumn4"];
                var score5 = scores["ProbabilityColumn5"];
                var score6 = scores["ProbabilityColumn6"];
                var score7 = scores["ProbabilityColumn7"];
                var bestPlay = scores["PredictedPlay"];

                $('#col1').html((score1 * 100).toFixed(2));
                $('#col2').html((score2 * 100).toFixed(2));
                $('#col3').html((score3 * 100).toFixed(2));
                $('#col4').html((score4 * 100).toFixed(2));
                $('#col5').html((score5 * 100).toFixed(2));
                $('#col6').html((score6 * 100).toFixed(2));
                $('#col7').html((score7 * 100).toFixed(2));
                $('#best').html(bestPlay+1);
                placeGamePieceByPosition(bestPlay, 0);
            });

        } else {
            gameBoardEl.addEventListener('click', placeGamePiece);
        }
    }


})();
