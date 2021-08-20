using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    public ParticleSystem emissionParticle;
    public ParticleSystem explosionParticle;
    public ParticleSystem fireParticle;
    public ParticleSystem coinParticle;
    public ParticleSystem fuelParticle;
    private Image fuelBarImg; 
    private Text coinText;
    public float moveSpeed;
    private float xPosLimit = 7.0f;
    private float rotationSpeed = 100.0f;
    public float maxSpeed;
    public float acceleration;
    public float horizontalTouchInput;
     private float currentHorizontalInput;
    private float lastRotationInput;
    private SpeedControl speedController;
    private GameManager gameManager;
    private bool shaking = false;
    private bool driving = false;
    private float shakeRange = 0.03f;
    private float driveAnimationRange = 0.02f;
    private Vector3 originalPos;
    private float lastXShake;
    private float fuelLeft = 100;
    public int coinsCollected = 0;
    private Rigidbody playerRb;
     private float testingInput;
    // Start is called before the first frame update
    void Start()
    {
        horizontalTouchInput = 0;
        currentHorizontalInput = 0;
        testingInput = 0;
        playerRb = GetComponent<Rigidbody>();
        fuelBarImg = GameObject.FindWithTag(CustomTags.FuelBar).GetComponent<Image>();
        coinText = GameObject.FindWithTag(CustomTags.CoinAmountText).GetComponent<Text>();
        gameManager = GameObject.FindWithTag(CustomTags.GameManager).GetComponent<GameManager>();
        speedController = GameObject.FindWithTag(CustomTags.SpeedControl).GetComponent<SpeedControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow)){
            testingInput -= 1;
        }
        if(Input.GetKeyUp(KeyCode.LeftArrow)){
            testingInput += 1;
        }
        if(Input.GetKeyDown(KeyCode.RightArrow)){
            testingInput += 1;
        }
        if(Input.GetKeyUp(KeyCode.RightArrow)){
            testingInput -= 1;
        }
        if(shaking) {
            EngineShake();
            return;
        }
        if(driving){
            // float horizontalInput = Input.GetAxis("Horizontal");
            // Debug.Log(horizontalInput);
            // float horizontalInput = CrossPlatformInputManager.GetAxis("Horizontal");
            //for testing
            // float input = 0;

            CalculateHorizontalInput(horizontalTouchInput);
            // CalculateHorizontalInput(testingInput);
            DoMovement(currentHorizontalInput);
            DoRotation(currentHorizontalInput);
        }
        fuelBarImg.fillAmount = fuelLeft / 100;
    }

    private void CalculateHorizontalInput(float horizontalInput){
        if((currentHorizontalInput > 0 && horizontalInput == -1)
        || (currentHorizontalInput < 0 && horizontalInput == 1)) currentHorizontalInput = 0;
        if(currentHorizontalInput > horizontalInput) currentHorizontalInput -= 0.065f;
        if(currentHorizontalInput < horizontalInput) currentHorizontalInput += 0.065f;
        
        if(Mathf.Abs(currentHorizontalInput - horizontalInput) < 0.075f)
            currentHorizontalInput = horizontalInput;
    }
    public void StartCar(){
        StartCoroutine(StartEngine());
    }
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag(CustomTags.Coin)){
            other.gameObject.SetActive(false);
            coinParticle.Play();
            coinsCollected++;
            coinText.text = coinsCollected.ToString();
        }else if(other.CompareTag(CustomTags.Fuel)){
            other.gameObject.SetActive(false);
            fuelParticle.Play();
            if(fuelLeft >= 75.0f) fuelLeft = 100.0f;
            else fuelLeft += 25.0f;
            speedController.AccelerateBy(acceleration);
        }
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag(CustomTags.Obstacle) && driving){
            driving = false;
            explosionParticle.Play();
            StartCoroutine(StartFireParticle());
            emissionParticle.Stop();
            playerRb.constraints = RigidbodyConstraints.None;
            other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            speedController.Stop();
            gameManager.StopGame();

            playerRb.AddForce(Vector3.forward * 500000, ForceMode.Impulse);
            playerRb.AddForce(Vector3.up * 500000, ForceMode.Impulse);
            if(transform.rotation.eulerAngles.y > 0.0f && transform.rotation.eulerAngles.y < 35.0f)
                playerRb.AddTorque(Vector3.up * 5000000, ForceMode.Impulse);
            else if(transform.rotation.eulerAngles.y < 360.0f && transform.rotation.eulerAngles.y > 325.0f)
                playerRb.AddTorque(Vector3.down * 5000000, ForceMode.Impulse);
            playerRb.AddTorque(Vector3.right * 50000000, ForceMode.Impulse);
        }
    }
    //Movement Functions
    private void DoRotation(float horizontalInput){
        if(transform.localPosition.x == -xPosLimit || transform.localPosition.x == xPosLimit)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0,0 ,0), Time.deltaTime * rotationSpeed * 2);
        else if(transform.rotation.eulerAngles.y < 360.0f && transform.rotation.eulerAngles.y > 330.0f && horizontalInput > 0.0f)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0,0 ,0), Time.deltaTime * rotationSpeed * 2);
        else if(transform.rotation.eulerAngles.y > 0.0f && transform.rotation.eulerAngles.y < 30.0f && horizontalInput < 0.0f)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0,0 ,0), Time.deltaTime * rotationSpeed * 2);
        else if(horizontalInput == -1.0f || (horizontalInput < lastRotationInput && horizontalInput < 0.0f))
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0,-30,0), Time.deltaTime * rotationSpeed);
        else if(horizontalInput == 1.0f || (horizontalInput > lastRotationInput && horizontalInput > 0.0f))
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0,30,0), Time.deltaTime * rotationSpeed);
        else
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0,0 ,0), Time.deltaTime * rotationSpeed * 2);
        lastRotationInput = horizontalInput;
    }
    private void DoMovement(float horizontalInput){
        //todo refine speed calculation
        //Debug.Log((moveSpeed + (speedController.currentSpeed / 5)));
        transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * (moveSpeed + (speedController.currentSpeed / 5)), Space.World);
        if(transform.localPosition.x < -xPosLimit) transform.position = new Vector3(-xPosLimit, transform.localPosition.y, transform.localPosition.z);
        if(transform.localPosition.x > xPosLimit) transform.position = new Vector3(xPosLimit, transform.localPosition.y, transform.localPosition.z);
    }

    //Shaking functions for when the car engine starts
    private void EngineShake(){
        float newXPos;
        if(lastXShake == 0.0f) newXPos = Random.Range(-shakeRange, shakeRange);
        else if(lastXShake > 0.0f) newXPos = Random.Range(-shakeRange, 0.0f);
        else newXPos = Random.Range(0.0f, shakeRange);
        lastXShake = newXPos;
        Vector3 newPos = new Vector3(newXPos,0,0) + originalPos;
        transform.position = newPos;
    }

    IEnumerator StartEngine(){
        originalPos = transform.localPosition;
        lastXShake = 0.0f;
        if(!shaking) shaking = true;
        yield return new WaitForSeconds(0.5f);
        shaking = false;
        driving = true;
        transform.position = originalPos;
        speedController.AccelerateBy(20.0f);
        StartCoroutine(DriveAnimation());
        StartCoroutine(DrainFuelLoop());
        emissionParticle.Play();
    }
    IEnumerator DrainFuelLoop(){
        WaitForSeconds delay = new WaitForSeconds(0.5f);
        while(driving){
            fuelLeft -= 1;
            
            if(fuelLeft <= 0){
                speedController.Stop();
                gameManager.StopGame();
                driving = false;
                emissionParticle.Stop();
                playerRb.constraints = RigidbodyConstraints.None;
                playerRb.AddForce(Vector3.forward * 500000, ForceMode.Impulse);
            }
            yield return delay;
        }
    }
    IEnumerator DriveAnimation(){
        while(driving){
            float timeBetween = 0.2f - (0.002f * speedController.currentSpeed);
            float originalY = transform.localPosition.y;
            if(driving) transform.position = transform.localPosition + new Vector3(0,driveAnimationRange, 0);
            yield return new WaitForSeconds(timeBetween);
            if(driving) transform.position = transform.localPosition - new Vector3(0, driveAnimationRange, 0);
            yield return new WaitForSeconds(timeBetween);
        }
    }

    IEnumerator StartFireParticle(){
        yield return new WaitForSeconds(2.0f);
        fireParticle.Play();
    }
}
