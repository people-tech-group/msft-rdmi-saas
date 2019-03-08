import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PostloginComponent } from './postlogin.component';

describe('PostloginComponent', () => {
  let component: PostloginComponent;
  let fixture: ComponentFixture<PostloginComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PostloginComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PostloginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
