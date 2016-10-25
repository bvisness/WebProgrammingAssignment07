var baseUrl = "http://localhost:63312/api/mariolevel/";

var actions = ["walk", "jump", "run", "wait"];
var retries = 0;
var maxRetries = 10;

var DIED_MESSAGE = "Mario was taken away...";
var TRANSITION_SELECTOR = ".yoshi, .ground, .background";

function initGame() {
    marioPosition = 0;
    $('.start').attr('disabled', true);
    $(TRANSITION_SELECTOR).css('left', "0").removeClass('die win');
}

function finishGame() {
    $('.start').attr('disabled', false);
}

function gameRequest(action) {
    if (action === undefined) {
        action = actions[Math.floor(Math.random() * actions.length)];
    }

    $.ajax(baseUrl + action, {
        method: "GET",
        success: function (data, textStatus, jqXHR) {
            retries = 0;

            $('.message').html(data.Message);
            if (data.Message == DIED_MESSAGE) {
                moveForAction("die");
            } else {
                moveForAction(action);

                if (marioPosition < 100) {
                    window.setTimeout(function () {
                        gameRequest(data.NextStep);
                    }, durationForAction(action));
                } else {
                    moveForAction("win");
                    finishGame();
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            retries++;
            if (retries <= maxRetries) {
                $('.message').html(errorThrown + ". Trying again...");
                gameRequest(action);
            } else {
                moveForAction("die");
                $('.message').html("Too many errors...game over!");
            }
        }
    });
}

function durationForAction(action) {
    if (action == "walk") {
        return 1000;
    } else if (action == "jump") {
        return 800;
    } else if (action == "run") {
        return 800;
    } else if (action == "wait") {
        return 500;
    } else if (action == "die") {
        return 1000;
    } else {
        return 0;
    }
}

function moveForAction(action) {
    var duration = durationForAction(action);

    if (action == "walk") {
        marioPosition += 5;
        classAnimation('walk', duration);
    } else if (action == "jump") {
        marioPosition += 5;
        classAnimation('jump', duration);
    } else if (action == "run") {
        marioPosition += 10;
        classAnimation('run', duration);
    } else if (action == "die") {
        $('.yoshi').addClass('die');
        finishGame();
    } else if (action == "win") {
        $('.yoshi').addClass('win');
        finishGame();
    }

    if (marioPosition > 100) {
        marioPosition = 100;
    }

    $('.yoshi').css('left', marioPosition + "%");
    $('.ground').css('left', (-marioPosition / 2) + "%");
    $('.background').css('left', (-marioPosition / 4) + "%");
}

function classAnimation(className, duration) {
    $(TRANSITION_SELECTOR).addClass(className);
    window.setTimeout(function () {
        $(TRANSITION_SELECTOR).removeClass(className);
    }, duration);
}

$(document).ready(function () {
    $('.start').on('click', function () {
        initGame();
        gameRequest();
    });
});
