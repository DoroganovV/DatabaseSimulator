import { Component } from '@angular/core';
import { ApplicationInsights, DistributedTracingModes } from '@microsoft/applicationinsights-web';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'app';
  constructor(applicationInsights: ApplicationInsights) {
    if (applicationInsights.config.instrumentationKey) {
      applicationInsights.loadAppInsights();
    }
  }
}
