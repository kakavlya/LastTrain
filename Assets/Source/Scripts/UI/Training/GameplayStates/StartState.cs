namespace LastTrain.Training
{
    public class StartState : GTState
    {
        public StartState(GameplayTraining gt)
            : base(gt)
        { }

        public override void Enter()
        {
            GT.HideAll();
            GT.StartTraining.SetActive(true);
            Showed();
            GT.StartButton.onClick.AddListener(OnNext);
        }

        public override void Exit()
        {
            GT.StartButton.onClick.RemoveListener(OnNext);
            GT.StartTraining.SetActive(false);
            Left();
            base.Exit();
        }

        private void OnNext()
        {
            GT.FSM.Switch<CameraMovementState>();
        }
    }
}