using CharacterControllers.Enemies.Bosses;


namespace Controls.Bosses {    
    public class BossShieldShieldedControl : BasicBossControl {

		public ServerBossShieldedController Controller { get; set; }

        public override void OnStartServer() {   
            if (!isServer) return;
            SetAnimation("Idle", true);                                                               
        }

        protected override void Update() {}
    }
}
