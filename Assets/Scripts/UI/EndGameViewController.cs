using GMTK2025.App;
using SLS.UI;

namespace GMTK2025.UI
{
    public class EndGameViewController : ViewController<EndGameView, GameState>
    {
        public EndGameViewController(EndGameView view, GameState model) : base(view, model)
        {

        }

        protected override void OnInit()
        {
            model.OnLose += Show;
        }

        protected override void OnDispose()
        {
            model.OnLose -= Show;
        }

        protected override void OnShow()
        {
            view.Setup(new EndGameView.PresenterModel
            {
                DescriptionText = "You Lose!",
                RestartText = "Restart",
                OnRestart = delegate
                {
                    model.StartGame();
                    Hide();
                },
            });
        }

        protected override void OnHide()
        {

        }
    }
}