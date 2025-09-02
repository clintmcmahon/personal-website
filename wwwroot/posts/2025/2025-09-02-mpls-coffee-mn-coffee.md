---
title: "MPLS Coffee is now MN Coffee"
description: "This month I'm releasing a big set of updates to MPLS Coffee. The biggest is a facelift and a name change from MPLS Coffee to MN Coffee. "
date: "2025-09-02"
draft: false
slug: "mpls-coffee-mn-coffee"
tags: [coffee,development]
---

<section>
 <div class="row">
      <div class="col-12">
        <article>
            <p>
               This past month I've added a bunch of new updates to my coffee app, MN Coffee. The two big changes are a complete facelift of the UI and changing the name from <a href="https://mplscoffee.com">MPLS Coffee</a> to MN Coffee. The <a href="https://mplscoffee.com">website</a> and codebase still refer to MPLS Coffee, however the official app name in the <a href="https://apps.apple.com/us/app/mn-coffee/id6736864166?platform=iphone">iOS</a> App Store has been updated to MN Coffee. I've yet to get the <a href="https://play.google.com/store/apps/details?id=com.parkasoftware.mplscoffee&hl=en_US">Android</a> store listing updated to MN Coffee. I hope to get that done this month.
            </p>
            <hr />
            <section class="mb-4">
            <h2>MPLS Coffee ---> MN Coffee</h2>
            <p>
               This name change should have been done from the beginning. When I first wrote the app, I just wanted to find the best coffee shops in Minneapolis. I soon realized that I wanted to include St. Paul as well. Then I wanted to make the app something that I could pop open no matter where in the state I was and still be able to find good coffee.  
            </p>
            <p>
              I feel like the new name is more inclusive and accurately reflects the app and idea. 
            </p>
          </section>
          <section class="mb-4">
            <h2 class="mb-3">Major UI Changes</h2>
              <div class="row">
                <div class="col-6 col-lg-3 mb-3">
                  <a href="/images/2025/mn_coffee_1.png" class="glightbox" data-gallery="mn-coffee-ui" data-title="MN Coffee App - Home Screen">
                    <img src="/images/2025/mn_coffee_1.png" alt="Minnesota Coffee Find App 1" class="img-fluid"/>
                  </a>
                </div>
                <div class="col-6 col-lg-3 mb-3">
                  <a href="/images/2025/mn_coffee_2.png" class="glightbox" data-gallery="mn-coffee-ui" data-title="MN Coffee App - Coffee Shop Details">
                    <img src="/images/2025/mn_coffee_2.png" alt="Minnesota Coffee Find App 2" class="img-fluid"/>
                  </a>
                </div>
                <div class="col-6 col-lg-3 mb-3">
                  <a href="/images/2025/mn_coffee_3.png" class="glightbox" data-gallery="mn-coffee-ui" data-title="MN Coffee App - Map View">
                    <img src="/images/2025/mn_coffee_3.png" alt="Minnesota Coffee Find App 3" class="img-fluid"/>
                  </a>
                </div>
                <div class="col-6 col-lg-3 mb-3">
                  <a href="/images/2025/mn_coffee_4.png" class="glightbox" data-gallery="mn-coffee-ui" data-title="MN Coffee App - Search Results">
                    <img src="/images/2025/mn_coffee_4.png" alt="Minnesota Coffee Find App 4" class="img-fluid"/>
                  </a>
                </div>
              </div>
              <p>
                The new UI changes are a big leap forward for getting the right information in front of the users without having them click so much. 
              </p>
              <ul>
                <li>I added a little toast when the user clicks a coffee shop. Inside that toast/bottom sheet is the coffee shop name, hours, reviews, AI summary, and then different highlighted features that came from Google. If the coffee shop was marked as having "Great Coffee" an icon with the Great Coffee label appears. The same goes for a variety of other elements such as Laptop Friendly, Pet Policy, Wifi, and if they have outdoor seating.
                </li>
                <li>
                  The data export from Outscraper provides a long list of properties that are helpful to end users. Here's that list and what is now displayed - by category - on the coffee shop detail page:
                  <div class="table-responsive mt-3">
                    <table class="table table-striped table-sm align-middle">
                      <caption class="text-muted">Location details (showing only enabled attributes)</caption>
                      <thead class="table-light">
                        <tr>
                          <th scope="col">Category</th>
                          <th scope="col">Details</th>
                        </tr>
                      </thead>
                      <tbody>
                        <tr>
                          <th scope="row">Service options</th>
                          <td>Curbside pickup, Takeout, Dine-in</td>
                        </tr>
                        <tr>
                          <th scope="row">Highlights</th>
                          <td>Great coffee, Great tea selection</td>
                        </tr>
                        <tr>
                          <th scope="row">Popular for</th>
                          <td>Breakfast, Solo dining, Good for working on laptop</td>
                        </tr>
                        <tr>
                          <th scope="row">Accessibility</th>
                          <td>Wheelchair accessible entrance, Wheelchair accessible restroom, Wheelchair accessible seating</td>
                        </tr>
                        <tr>
                          <th scope="row">Offerings</th>
                          <td>Coffee, Quick bite</td>
                        </tr>
                        <tr>
                          <th scope="row">Dining options</th>
                          <td>Breakfast, Dessert, Seating</td>
                        </tr>
                        <tr>
                          <th scope="row">Amenities</th>
                          <td>Gender-neutral restroom, Restroom, Wi-Fi</td>
                        </tr>
                        <tr>
                          <th scope="row">Atmosphere</th>
                          <td>Casual, Cozy, Trendy</td>
                        </tr>
                        <tr>
                          <th scope="row">Crowd</th>
                          <td>Family-friendly, LGBTQ+ friendly, Transgender safespace</td>
                        </tr>
                        <tr>
                          <th scope="row">Planning</th>
                          <td>Accepts reservations: <span class="text-muted">No</span></td>
                        </tr>
                        <tr>
                          <th scope="row">Payments</th>
                          <td>Credit cards, Debit cards, NFC mobile payments</td>
                        </tr>
                        <tr>
                          <th scope="row">Children</th>
                          <td>Good for kids</td>
                        </tr>
                        <tr>
                          <th scope="row">Parking</th>
                          <td>Free street parking</td>
                        </tr>
                        <tr>
                          <th scope="row">Pets</th>
                          <td>Dogs allowed</td>
                        </tr>
                        <tr>
                          <th scope="row">Other</th>
                          <td>LGBTQ+ friendly</td>
                        </tr>
                      </tbody>
                    </table>
                  </div>
                </li>
                <li>
                  The updated list page displays the same information but in a list format that is sorted by closest to the user. I included the same details as the toast popup as well as the address, get directions arrow, and the AI summary information provided by Google.
                </li>
                <li>
                  Then there is the updated about screen. Nothing really big to talk about there other than what the app is about and why I think it's cool. Standard about page stuff here.
                </li>
              </ul>
              <h2>That's it for this month</h2>
              <p>
                I'm having a lot of fun building this app when I have the time. Between <a href="/services/custom-software-development">consulting</a> and <a href="https://biglittlecities.com">Big Little Cities</a>, I have a couple of hours per month to work on this. Claude AI coding agents have been a pretty big help as well. Using AI to develop alongside me has been super efficient but also really frustrating at times. Overall, I think it's a net positive, though. Maybe I will write a post about that sometime soon.
              </p>
          </section>
        </article>
      </div>
    </div>
</section>