using GMTK2025.App;
using SLS.UI;

namespace GMTK2025.UI
{
    public class StartGameViewController : ViewController<StartGameView, GameState>
    {
        protected override bool showOnInit => true;

        public StartGameViewController(StartGameView view, GameState model) : base(view, model)
        {

        }

        protected override void OnInit()
        {
            
        }

        protected override void OnDispose()
        {
            
        }

        protected override void OnShow()
        {
            view.Setup(new StartGameView.PresenterModel
            {
                StartText = "Start New Game",
                OnStart = delegate
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