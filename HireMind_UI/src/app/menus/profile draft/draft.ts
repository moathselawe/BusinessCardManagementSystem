
//<p-dialog header = "My Profile"
//[(visible)] = "userDialogVisible"
//[modal] = "true"
//[draggable] = "false"
//[resizable] = "false"
//[style] = "{ width: '700px' }"
//styleClass = "user-profile-dialog" >

//  <div class="profile-wrapper" >

//    <!--LEFT: AVATAR + BASIC-- >
//      <div class="profile-left" >

//        <div class="avatar-circle-large" >
//          <img src="https://images.unsplash.com/photo-1500648767791-00dcc994a43e?w=200"
//alt = "User Avatar"
//style = "width:100px;height:100px;border-radius:50%;object-fit:cover;" >
//  </div>

//  < h3 class="name" > {{ user?.nameEnglish }}</h3>
//    < span class="sub" > {{ user?.email }}</span>


//      < button pButton
//label = "Logout"
//icon = "pi pi-sign-out"
//class="p-button-sm p-button-danger mt-2" >
//  </button>

//  </div>

//  < !--RIGHT: DETAILS-- >
//    <div class="profile-right" >

//      <!--Identity -->
//        <div class="section" >
//          <h4>Identity </h4>

//          < div class="grid" >
//            <div><b>Arabic Name: </b> {{ user?.nameArabic }}</div >
//              <div><b>English Name: </b> {{ user?.nameEnglish }}</div >
//                <div><b>Gender: </b> {{ user?.gender }}</div >
//                  </div>
//                  </div>

//                  < !--Contact -->
//                    <div class="section" >
//                      <h4>Contact </h4>

//                      < div class="grid" >
//                        <div><b>Mobile: </b> {{ user?.mobile }}</div >
//                          <div><b>Email: </b> {{ user?.email }}</div >
//                            <div><b>Address: </b> {{ user?.address || '-' }}</div >
//                              </div>
//                              </div>

//                              < !--Account Status-- >
//                                <div class="section" >
//                                  <h4>Account Status </h4>

//                                    < div class="grid" >
//                                      <div><b>Active: </b> {{ user?.isActive ? 'Yes' : 'No' }}</div >
//                                        <div><b>Locked: </b> {{ user?.isLocked ? 'Yes' : 'No' }}</div >
//                                          <div><b>Failed Attempts: </b> {{ user?.failedLoginAttempts }}</div >
//                                            <div><b>Locked Date: </b> {{ user?.lockedDate | date:'short' }}</div >
//                                              </div>
//                                              </div>

//                                              < !--Roles -->
//                                                <div class="section" >
//                                                  <h4>Roles </h4>

//                                                  < div class="roles" >
//                                                    <span class="role-chip" * ngFor="let r of user?.roleIds" >
//                                                      {{ r }}
//</span>
//  </div>
//  </div>

//  </div>

//  </div>

//  </p-dialog>




//userDialogVisible = false;

//user: User = {
//  id: '1',
//  nameArabic: 'محمد',
//  nameEnglish: 'Moath Selawe',
//  mobile: '+971 50 000 0000',
//  address: 'Abu Dhabi',
//  email: 'moath@email.com',
//  gender: Gender.Male,
//  isActive: true,
//  isLocked: false,
//  lockedDate: new Date(),
//  failedLoginAttempts: 0,
//  roleIds: ["Admin", "Developer"]
//}; 
