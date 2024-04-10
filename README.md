# HospitalAPI

#### Note to self -- View or Swagger?
originally planned to use sawgger, but reviewed car dealership project, wonder whether view is a better choice? ---check with Karen
#### Note to self--TreatmentPlan Entry
treatment plan entry can be a self generated, check note in TreamentPlan.cs
#### Note to self--TreatmentPlan Entry
Medication plan entry can be a self generated, check note in MedicationPlan.cs

#### Note to self
deleted patient_emergency_contact att. to simplify the process, can add back later, if add back, 
it should have a link? stored in a array? as contact will have few attributes
#### Note to self
drug does not necessary need a controller. but could have one? But not with all CRUD operations though, just R(read) for retrieving a drug per drugId.can think about it.

#### Note to self
Note: DBcontext is not a retrievable object
#### Note to self
#### Note to self
#### Note to self
- Lists—Might use CSV files for drug list, patient list?
For Medication Plan and Treatment Plan, will likely be a list/array where doctors can add items .

#### Note to self-Git
sign in github, 

To save to git, open powershell, go to project directory (C:\Users\User\Desktop\My development\HospitalAPI)
- git status //display what's new
- git add * //add all the work in progress
- git commit -m "put a message here" //add a message to your commit history
- git push
31/03: fixed dependencies between treatmentplan, medicationplan and testresut, I may find more change needed when start doing UI,
- ie, might need to add specific quesries in regards to functionalities, ie, to sort patient list by age(DOB)

