import { Component, ElementRef, ViewChild } from '@angular/core';
import { BusinessCard } from '../../../models/businessCard';
import { ActivatedRoute, Router } from '@angular/router';
import { BusinessCardService } from '../../../services/businessCard.service';
import { NgForm } from '@angular/forms';
import { ToastMessageService } from '../../../services/shared/toast-message.service';
interface Theme {
  name: string;
  background: string;
  fontColor: string;
}

@Component({
  selector: 'app-create-businesscards-component',
  standalone: false,
  templateUrl: './create-businesscards-component.html',
  styleUrls: ['./create-businesscards-component.css'],
})
export class CreateBusinesscardsComponent {
  cards: BusinessCard[] = [];
  selectedCardIndex: number = 0;
  isEditMode: boolean = false;
  imageError: boolean = false;
  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;

  readonly: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private service: BusinessCardService,
    private toastService: ToastMessageService
  ) { }

  isEdit: boolean = false;
  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    const readonly = this.route.snapshot.queryParamMap.get('readonly') === 'true';
    this.readonly = readonly;

    const navState = history.state.previewCards;
    if (navState && Array.isArray(navState)) {
      this.cards = navState.map((c: any) => {
        const bc = new BusinessCard();
        Object.assign(bc, c);
        if (bc.dateOfBirth) bc.dateOfBirth = new Date(bc.dateOfBirth);
        return bc;
      });

      if (this.cards.length) {
        this.isEdit = true;
      }
    }

    if (!this.cards.length) {
      this.cards.push(new BusinessCard());
    }

    if (id) {
      this.isEditMode = !readonly;
      this.isEdit = true;
      this.loadCard(id);
    }
  }

  get card(): BusinessCard {
    return this.cards[this.selectedCardIndex];
  }

  selectCard(index: number) {
    this.selectedCardIndex = index;
  }

  loadCard(id: string) {
    this.service.GetById(id).subscribe({
      next: (res: any) => {
        this.cards = [res.response];
        if (this.cards[0].dateOfBirth) {
          this.cards[0].dateOfBirth = new Date(this.cards[0].dateOfBirth);
        }
      },
      error: (err) => console.error('Failed to load card', err),
    });
  }

  validateCard(card: BusinessCard, index: number): boolean {

    const errors: string[] = [];

    if (!card.arabicName || card.arabicName.trim().length < 5)
      errors.push(`Card #${index + 1}: Arabic Name is required (min 5 characters).`);

    if (!card.englishName || card.englishName.trim().length < 5)
      errors.push(`Card #${index + 1}: English Name is required (min 5 characters).`);

    if (!card.email || !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(card.email))
      errors.push(`Card #${index + 1}: Valid Email is required.`);

    if (!/^\+?\d{7,15}$/.test(card.phone))
      errors.push(`Card #${index + 1}: Phone must be 7–15 digits, optionally starting with +.`);

    if (!card.address || card.address.trim().length < 10)
      errors.push(`Card #${index + 1}: Address is required (min 10 characters).`);

    if (!card.dateOfBirth)
      errors.push(`Card #${index + 1}: Date of Birth is required.`);

    if (errors.length > 0) {
      alert(errors.join("\n"));
      this.isEdit = true; 
      return false;
    }

    return true;
  }

  saveCard() {
    if (!this.validateCard(this.card, this.selectedCardIndex)) return; 

    this.imageError = false;

    if (this.isEditMode) {
      this.service.Update(this.card).subscribe(
        {
          next: () => {
            this.toastService.showMessage({
              messageType: 'success',
              messageTitle: 'Updated',
              messageBody: 'Business card updated successfully.'
            });
            this.router.navigate(['/BCMS/ManageBusinesscards']);
          } ,error: (err) => {
            this.toastService.showMessage({
              messageType: 'error',
              messageTitle: 'Error',
              messageBody: 'Failed to update business card.'
            });
            console.error('Update failed', err);
          },
        });
    } else {
      this.service.Add(this.card).subscribe({
        next: () => {
          this.toastService.showMessage({
            messageType: 'success',
            messageTitle: 'Created',
            messageBody: 'Business card created successfully.'
          });
          this.router.navigate(['/BCMS/ManageBusinesscards']);
        },
        error: (err) => {
          this.toastService.showMessage({
            messageType: 'error',
            messageTitle: 'Error',
            messageBody: 'Failed to create business card.'
          });
          console.error('Creation failed', err);
        },
      });
    }
  }

  saveCards() {
    for (let i = 0; i < this.cards.length; i++) {
      if (!this.validateCard(this.cards[i], i)) return;
    }

    this.imageError = false;

    const payload = this.cards.map(c => ({
      ArabicName: c.arabicName,
      EnglishName: c.englishName,
      DateOfBirth: c.dateOfBirth ? c.dateOfBirth.toISOString() : null,
      Email: c.email,
      Phone: c.phone.startsWith('+') ? c.phone : '+' + c.phone,
      Logo: c.logo,
      Address: c.address
    }));

    this.service.CreateMany(payload).subscribe({
      next: () => {
        this.toastService.showMessage({
          messageType: 'success',
          messageTitle: 'Created',
          messageBody: 'All business cards were created successfully.'
        });
        this.router.navigate(['/BCMS/ManageBusinesscards']);
      },
      error: (err) => {
        this.toastService.showMessage({
          messageType: 'error',
          messageTitle: 'Error',
          messageBody: 'Failed to create business cards.'
        });
        console.error('Creation failed', err);
      },
    });
  }


  resetCard(form?: NgForm) {
    this.cards[this.selectedCardIndex] = new BusinessCard();
    this.imageError = false;
    if (form) form.resetForm();
    if (this.fileInput) this.fileInput.nativeElement.value = '';
  }

  goBack() {
    this.router.navigateByUrl('/BCMS/ManageBusinesscards');
  }

  maxImageSizeMB = 1;
  maxWidth = 1200;
  maxHeight = 1200;

  fileSelected(event: any) {
    const file = event.target.files[0];
    if (!file) return;

    if (!file.type.startsWith('image/')) {
      this.imageError = true;
      return;
    }

    this.compressImageFile(file, this.maxImageSizeMB)
      .then(({ base64 }) => {
        this.card.logo = base64;
        this.imageError = false;
      })
      .catch(err => {
        console.error(err);
        this.imageError = true;
      });
  }

  compressImageFile(file: File, maxMB: number): Promise<{ base64: string, size: number }> {
    const maxBytes = maxMB * 1024 * 1024;
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.onload = () => {
        const img = new Image();
        img.onload = () => {
          let { width, height } = img;
          let ratio = Math.min(this.maxWidth / width, this.maxHeight / height, 1);
          const canvas = document.createElement('canvas');
          canvas.width = Math.round(width * ratio);
          canvas.height = Math.round(height * ratio);
          const ctx = canvas.getContext('2d')!;
          ctx.drawImage(img, 0, 0, canvas.width, canvas.height);

          let quality = 0.9;
          let base64 = canvas.toDataURL('image/jpeg', quality);
          let size = this._base64ToBytes(base64).length;

          while (size > maxBytes && quality > 0.2) {
            quality -= 0.1;
            base64 = canvas.toDataURL('image/jpeg', quality);
            size = this._base64ToBytes(base64).length;
          }

          if (size > maxBytes) {
            return reject(new Error('Cannot compress image below target size.'));
          }

          resolve({ base64, size });
        };
        img.onerror = reject;
        img.src = reader.result as string;
      };
      reader.onerror = reject;
      reader.readAsDataURL(file);
    });
  }

  _base64ToBytes(base64: string): Uint8Array {
    const comma = base64.indexOf(',');
    const raw = base64.substr(comma + 1);
    const binary = atob(raw);
    const bytes = new Uint8Array(binary.length);
    for (let i = 0; i < binary.length; i++) bytes[i] = binary.charCodeAt(i);
    return bytes;
  }

  themes: Theme[] = [
    { name: 'Corporate', background: 'linear-gradient(135deg, #f0f4ff 0%, #dce6ff 100%)', fontColor: '#333' },
    { name: 'Creative', background: 'linear-gradient(135deg, #ffedf0 0%, #ffc1e3 100%)', fontColor: '#333' },
    { name: 'Minimal', background: 'linear-gradient(135deg, #ffffff 0%, #f0f0f0 100%)', fontColor: '#333' },
    { name: 'Dark', background: 'linear-gradient(135deg, #333 0%, #555 100%)', fontColor: '#fff' }
  ];

  selectedThemeName: string = this.themes[0].name;
  filteredThemes: string[] = [];

  get selectedTheme(): Theme {
    return this.themes.find(t => t.name === this.selectedThemeName)!;
  }

  search(event: any) {
    const query = event.query.toLowerCase();
    this.filteredThemes = this.themes
      .map(t => t.name)
      .filter(name => name.toLowerCase().includes(query));
  }

  defaultUserIcon = 'data:image/svg+xml;base64,PHN2ZyBmaWxsPSIjY2NjIiBoZWlnaHQ9IjQ4IiB3aWR0aD0iNDgiIHZpZXdCb3g9IjAgMCA0OCA0OCIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj48Y2lyY2xlIGN4PSIyNCIgY3k9IjE0IiByPSIxMCIvPjxwYXRoIGQ9Ik0yNCAyNmMtOC44IDAtMTYgNy4yLTE2IDE2aDMyaC0wLjAwMUM0MCAzMy4yIDMyLjggMjYgMjQgMjZ6Ii8+PC9zdmc+';
  defaultQRCodeIcon = 'data:image/svg+xml;base64,PHN2ZyBmaWxsPSIjRTBFMEUwIiB3aWR0aD0iMTAwIiBoZWlnaHQ9IjEwMCIgdmlld0JveD0iMCAwIDEwMCAxMDAiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyI+PHJlY3Qgd2lkdGg9IjEwMCIgaGVpZ2h0PSIxMDAiLz48dGV4dCB4PSI1MCUiIHk9IjUwJSIgZmlsbD0iIzAwMCIgZm9udC1zaXplPSIzMHB4IiBkb21pbmFudC1iYXNlbGluZT0ibWlkZGxlIiB0ZXh0LWFuY2hvcj0ibWlkZGxlIj5RUjwvdGV4dD48L3N2Zz4=';


  resetCardByIndex(index: number) {
    this.cards[index] = new BusinessCard();
    if (index === this.selectedCardIndex && this.fileInput) {
      this.fileInput.nativeElement.value = '';
    }
  }

  removeCard(index: number) {
    if (this.cards.length > 1) {
      this.cards.splice(index, 1);
      if (this.selectedCardIndex >= this.cards.length) {
        this.selectedCardIndex = this.cards.length - 1;
      }
    }
  }

  addCard() {
    this.cards.push(new BusinessCard());
    this.selectedCardIndex = this.cards.length - 1;
  }

  today: Date = new Date();
}


