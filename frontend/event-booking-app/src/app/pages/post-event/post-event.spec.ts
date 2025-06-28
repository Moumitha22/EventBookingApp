import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PostEvent } from './post-event';

describe('PostEvent', () => {
  let component: PostEvent;
  let fixture: ComponentFixture<PostEvent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PostEvent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PostEvent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
