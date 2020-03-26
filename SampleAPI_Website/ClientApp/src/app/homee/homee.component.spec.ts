/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { HomeeComponent } from './homee.component';

let component: HomeeComponent;
let fixture: ComponentFixture<HomeeComponent>;

describe('homee component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ HomeeComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(HomeeComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});