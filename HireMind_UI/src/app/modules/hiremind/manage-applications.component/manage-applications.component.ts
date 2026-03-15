import { Component } from '@angular/core';
import { ToastMessageService } from '../../../services/shared/toast-message.service';
import { ActivatedRoute, Router } from '@angular/router';
import { JobApplicationService } from '../../../services/hiremind/jobApplication.service';
import { GetJobApplication } from '../../../models/hiremind/GetJobApplication';

@Component({
  selector: 'app-manage-applications.component',
  standalone: false,
  templateUrl: './manage-applications.component.html',
  styleUrl: './manage-applications.component.css',
})
export class ManageApplicationsComponent {
  applications: GetJobApplication[] = [];
  loading: boolean = false;

  constructor(
    public service: JobApplicationService,
    private route: ActivatedRoute,
    private router: Router,
    public toastService: ToastMessageService
  ) { }

  ngOnInit(): void {

    const id = Number(this.route.snapshot.paramMap.get('id'));

    this.loading = true;

    this.service.getAllJobApplicationsByJobId(id).subscribe({
      next: (res: any) => {

        this.applications = res.response;
        this.loading = false;

      },
      error: () => {

        this.loading = false;

        this.toastService.showMessage({
          messageType: 'error',
          messageTitle: 'Error',
          messageBody: 'Failed to load Job Applications.'
        });
      }
    });
  }

}
