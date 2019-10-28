using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
public class movePersonagem : MonoBehaviour
{

    public GameObject jogador;
    public Animation animacao;

    public GameObject particulaOvo;
    public GameObject particulaPena;
    public GameObject particulaEstrela;
    public GameObject objetoParticulaFogo;

    CharacterController objetoCharControler;
    float velocidade = 2.0f;
    float giro = 3.0f;
    float frente = 3.0f;
    float pulo = 5.0f;

    Vector3 vetorDirecao = new Vector3(0, 0, 0);
    Vector3 moveCameraFrente;
    Vector3 moveMove;
    Vector3 normalZeroPiso = new Vector3(0, 0, 0);
    Transform transformCamera;

    float contaPisca = 0;
    bool podePegarStar;
    int numeroObjetos;

    public AudioClip somOvo;
    public AudioClip somPena;
    public AudioClip somEstrela;
    public AudioClip somHit;
    public AudioClip somWin;
    public AudioClip somLose;
    public AudioClip somApareceStar;
    public AudioClip somFelpudoVoa;


    // Use this for initialization
    void Start()
    {

        objetoCharControler = GetComponent<CharacterController>();
        animacao = jogador.GetComponent<Animation>();
        transformCamera = Camera.main.transform;

    }

    // Update is called once per frame
    void Update()
    {

        moveCameraFrente = Vector3.Scale(transformCamera.forward, new Vector3(1, 0, 1)).normalized;
        //para usar teclado use a linha abaixo INPUT
        //moveMove = Input.GetAxis("Vertical") * moveCameraFrente + Input.GetAxis("Horizontal") * transformCamera.right;

        //para usar joystic use a linha abaixo CrossPlatformInputManager
        moveMove = CrossPlatformInputManager.GetAxis("Vertical") * moveCameraFrente + CrossPlatformInputManager.GetAxis("Horizontal") * transformCamera.right;

        vetorDirecao.y -= 3.0f * Time.deltaTime;
        objetoCharControler.Move(vetorDirecao * Time.deltaTime);
        objetoCharControler.Move(moveMove * velocidade * Time.deltaTime);

        if (moveMove.magnitude > 1f) moveMove.Normalize();
        moveMove = transform.InverseTransformDirection(moveMove);

        moveMove = Vector3.ProjectOnPlane(moveMove, normalZeroPiso);
        giro = Mathf.Atan2(moveMove.x, moveMove.z);
        frente = moveMove.z;

        objetoCharControler.SimpleMove(Physics.gravity);
        aplicaRotacao();

        if (CrossPlatformInputManager.GetButton("Jump"))
        {
            if (objetoCharControler.isGrounded == true)
            {
                vetorDirecao.y = pulo;
                jogador.GetComponent<Animation>().Play("JUMP");
                GetComponent<AudioSource>().PlayOneShot(somFelpudoVoa, 0.7F);
            }
        }
        else
        {
            //if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
            if((CrossPlatformInputManager.GetAxis("Horizontal") != 0.0f) || (CrossPlatformInputManager.GetAxis("Vertical") != 0.0f) )
            {
                if (!animacao.IsPlaying("JUMP"))
                {
                    jogador.GetComponent<Animation>().Play("WALK");
                }
            }
            else
            {
                if (objetoCharControler.isGrounded == true)
                {
                    jogador.GetComponent<Animation>().Play("IDLE");
                }
            }
        }
    }

    void aplicaRotacao()
    {
        float turnSpeed = Mathf.Lerp(180, 360, frente);
        transform.Rotate(0, giro * turnSpeed * Time.deltaTime, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "OVO")
        {
            Instantiate(particulaOvo, other.gameObject.transform.position, Quaternion.identity);
            other.gameObject.SetActive(false);
            //pegar itens
            numeroObjetos++; verificaPickObjetos();
            GetComponent<AudioSource>().PlayOneShot(somOvo, 0.7F);
        }
        if (other.gameObject.tag == "PENA")
        {
            Instantiate(particulaPena, other.gameObject.transform.position, Quaternion.identity);
            other.gameObject.SetActive(false);
            //pegar itens
            numeroObjetos++; verificaPickObjetos();
            GetComponent<AudioSource>().PlayOneShot(somPena, 0.7F);
        }
        if (other.gameObject.tag == "ESTRELA")
        {
            if (podePegarStar) {
                GetComponent<AudioSource>().PlayOneShot(somEstrela, 0.7F);
                Instantiate(particulaEstrela, other.gameObject.transform.position, Quaternion.identity);
            other.gameObject.SetActive(false);
                GetComponent<AudioSource>().PlayOneShot(somWin, 0.7F);

                //recarrega fase apos 3 segundos apos pegar a estrela
                Invoke("carregaFase",3);
                
            }
        }
        if (other.gameObject.tag == "FOGUEIRA")
        {
			
			InvokeRepeating("mudaEstadoFelpudo", 0, 0.1f);

			//hit ao bater na fogueira'
            objetoCharControler.Move(transform.TransformDirection(Vector3.back) * 1);
            GetComponent<AudioSource>().PlayOneShot(somHit, 0.7F);
        }
        if (other.gameObject.tag == "BURACO")
        {
            Invoke("carregaFase", 2);
            GetComponent<AudioSource>().PlayOneShot(somLose, 0.7F);
        }
    }

    void mudaEstadoFelpudo()
    {
        contaPisca++;
        jogador.SetActive(!jogador.activeInHierarchy);
        if (contaPisca > 7) { contaPisca = 0;jogador.SetActive(true); CancelInvoke(); }
    }

    void verificaPickObjetos()
    {
        if (numeroObjetos > 4)
        {
            podePegarStar = true;
            Destroy(objetoParticulaFogo);
            GetComponent<AudioSource>().PlayOneShot(somApareceStar, 0.7F);
        }
    }
    void carregaFase()
    {
        Application.LoadLevel("CenaAventura");
    }
}
