import { Component, EventEmitter, NgZone, Output, ViewChild } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { CdkTextareaAutosize, TextFieldModule } from '@angular/cdk/text-field';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-chat-input',
  standalone: true,
  imports: [ReactiveFormsModule, MatInputModule, MatFormFieldModule, MatIconModule, TextFieldModule],
  templateUrl: './chat-input.component.html',
  styleUrl: './chat-input.component.scss'
})
export class ChatInputComponent {

  @ViewChild('autosize') autosize!: CdkTextareaAutosize;
  readonly formGroup = new FormGroup({
    message: new FormControl('')
  });
  @Output() messageSubmitted = new EventEmitter<string>();

  constructor(private readonly ngZone: NgZone) { }

  get isSubmitButtonDisabled() {
    return this.formGroup.controls.message.disabled || !this.formGroup.value.message;
  }

  triggerResize() {
    this.ngZone.onStable.pipe(take(1)).subscribe(() => this.autosize.resizeToFitContent(true));
  }

  onSubmit() {
    const message = this.formGroup.value.message;
    if (!message) {
      return;
    }

    this.messageSubmitted.emit(message);
    this.formGroup.reset();
  }

  onKeyDown(event: KeyboardEvent) {
    if (event.key === 'Enter' && event.ctrlKey) {
      event.preventDefault();
      this.onSubmit();
    }
  }

  disableInput() {
    this.formGroup.controls.message.disable();
  }

  enableInput() {
    this.formGroup.controls.message.enable();
  }
}
