import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule, Routes } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatChipsModule } from '@angular/material/chips';
import { MatOptionModule } from '@angular/material/core';
import { MatDialogModule } from '@angular/material/dialog';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatMenuModule } from '@angular/material/menu';
import { MatStepperModule } from '@angular/material/stepper';
import { MatFormFieldModule } from '@angular/material/form-field';

import { AppComponent } from './app.component';
import { SimulatorService } from './simulator/services/simulator-service';
import { LoginService } from './shared/components/login/login.service';

import { TableComponent } from './shared/components/table/table.component';
import { LoadingIcons } from './shared/components/loadingIcons/loadingIcons';

import { SimulatorHomeComponent } from './simulator/components/home/simulator-home.component';
import { LoginComponent } from './shared/components/login/login.component';
import { ApplicationInsights, DistributedTracingModes } from '@microsoft/applicationinsights-web';
import { environment } from '../environments/environment';

const appRoutes: Routes = [
  { path: '', component: SimulatorHomeComponent },
];

const applicationInsightsFactory = () => {
  let applicationInsights = window['applicationInsightsLogger'] as ApplicationInsights;

  if (!applicationInsights) {
    applicationInsights = window['applicationInsightsLogger'] = new ApplicationInsights({
      config: {
        instrumentationKey: environment.instrumentationKey,
        enableCorsCorrelation: true,
        enableAutoRouteTracking: true,
        distributedTracingMode: DistributedTracingModes.W3C,
        enableRequestHeaderTracking: true,
        enableResponseHeaderTracking: true,
        endpointUrl: environment.IngestionEndpoint
      }
    });
  }

  return applicationInsights;
};

@NgModule({
  declarations: [
    AppComponent,
    SimulatorHomeComponent,
    TableComponent,
    LoadingIcons,
    LoginComponent
  ],
  exports: [
    MatIconModule, MatSortModule, MatTabsModule, MatRadioModule, MatTooltipModule, MatSelectModule, MatOptionModule, MatCardModule, MatInputModule, MatAutocompleteModule,
    MatCheckboxModule, MatDialogModule, MatExpansionModule, MatButtonModule, MatProgressSpinnerModule, MatSidenavModule, MatTableModule, MatPaginatorModule, MatChipsModule,
    MatStepperModule, MatMenuModule, MatFormFieldModule
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    RouterModule.forRoot(appRoutes),

    MatIconModule, MatSortModule, MatTabsModule, MatRadioModule, MatTooltipModule, MatSelectModule, MatOptionModule, MatCardModule, MatInputModule, MatAutocompleteModule,
    MatCheckboxModule, MatDialogModule, MatExpansionModule, MatButtonModule, MatProgressSpinnerModule, MatSidenavModule, MatTableModule, MatPaginatorModule, MatChipsModule,
    MatStepperModule, MatMenuModule, MatFormFieldModule
  ],
  providers: [
    SimulatorService,
    LoginService,
    { provide: ApplicationInsights, useFactory: applicationInsightsFactory }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
