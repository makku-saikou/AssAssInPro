using Pditine.Audio;
using Pditine.Data;
using Pditine.GamePlay.Buff;
using Pditine.GamePlay.Camera;
using Pditine.Player;
using Pditine.Player.Ass;
using Pditine.Player.Thorn;
using Pditine.Utility;

namespace Pditine.Collide.CollideEvent
{
    public class Thorn_AssEvent : CollidingEventBase
    {
        protected override bool CompareTag(string tag1, string tag2)
        {
            if (tag1 == "Thorn" && tag2 == "Ass") return true;
            return false;
        }

        protected override void Happen(ColliderBase collider1, ColliderBase collider2,CollideInfo info)
        {
            var thePlayer1 = (collider1 as ThornBase).ThePlayer;
            var thePlayer2 = (collider2 as AssBase).ThePlayer;

            //todo: 伤害计算放在这里不合适
            float demage = thePlayer1.ATK * thePlayer1.CurrentSpeed / thePlayer1.SpeedCoefficient;
            thePlayer2.ChangeHP(-demage);
            //thePlayer1.HitFeedback();
            var res =
                PhysicsUtility.ElasticCollision(thePlayer1.CurrentDirection * thePlayer1.CurrentSpeed,
                    thePlayer2.CurrentDirection * thePlayer2.CurrentSpeed,
                    thePlayer1.Weight, thePlayer2.Weight, thePlayer1.transform.position, thePlayer2.transform.position);
            thePlayer1.CurrentDirection = res.v1Prime.normalized;
            thePlayer2.CurrentDirection = res.v2Prime.normalized;
            thePlayer1.CurrentSpeed = res.v1Prime.magnitude;
            thePlayer2.CurrentSpeed = res.v2Prime.magnitude;
            
            AAIAudioManager.Instance.PlayEffect("碰撞音效1");
            // if(demage>10)
            //     CameraManagerBase.Instance.OnCollidePLayerAss(thePlayer2.ID);
            
            thePlayer2.VFX[VFXName.AssHit].Play();
            
            BuffManager.Instance.AttachBuff(new BuffInfo(DataManager.Instance.GetBuffData(9),null,thePlayer2));
            
            (collider1 as ThornBase).OnAttack?.Invoke();
            (collider2 as AssBase).OnBeAttackByThorn?.Invoke(collider1);
            (collider2 as AssBase).OnBeAttack?.Invoke(collider1);
        }
    }
}