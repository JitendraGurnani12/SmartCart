import { Component } from '@angular/core';
import { RegisterRequest } from '../../models/register-request.model';
import { AuthApiService } from '../../services/auth-api.service';
import { Router } from '@angular/router';
import { ToasterService } from 'src/app/shared/toaster/service/toaster.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
registerDto: RegisterRequest = {
    displayName: '',
    email: '',
    password: '',
    addresses: ''
  };
  constructor(private authApiService : AuthApiService,
    private router : Router,
    private toasterService : ToasterService
  ){

  }
  ngOnInit(){

  }

  submitted = false;

  onSubmit(): void {

    this.submitted = true;

    if (
      !this.registerDto.displayName ||
      !this.registerDto.email ||
      !this.registerDto.password ||
      !this.registerDto.addresses
    ) {
      return;
    }


    console.log('Register Request', this.registerDto);
    this.authApiService.register(this.registerDto).subscribe(data=>{
     
      this.toasterService.showSuccessToast("Registered Successfully");
      this.router.navigate(['auth/login']);
    },
    (error)=>{
      this.toasterService.showWarningToast(error);
    }
  
  );

    // API call later
  }
}
