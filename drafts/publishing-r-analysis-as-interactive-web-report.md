---
title: "Publishing R Analysis as an Interactive Web Report"
date: "2026-08-08"
slug: "publishing-r-analysis-as-interactive-web-report"
description: "How to get R or SAS output into a fast interactive web application without the application recomputing any of the statistics."
keywords: "publish r output web application, r analysis interactive report, shiny alternative production, research data visualization web app, registry reporting platform"
tags: "data, dotnet, react, healthcare"
draft: true
---

<!--
WRITING PROMPT

WHAT THIS POST IS ABOUT:
A research group has analysis in R and publishes it as PDFs. Someone has asked
why it cannot be interactive. They are evaluating whether to try Shiny, hire
someone or give up. This post explains the architecture that works and the
constraint that makes it different from a normal dashboard project.

Almost nobody competes for this. It is a small audience but every reader is a
qualified buyer for exactly the work you want more of.

THE ANGLE THAT MAKES THIS YOURS:
SRTR and USRDS. You have shipped this pattern on federal health data that
clinicians use for real decisions. The specific insight worth leading with is
the one you already put on the case study: the web application must not
recompute anything, because disagreeing with the published result is a serious
failure, not a bug.

QUESTIONS TO WORK THROUGH:
1. What does the handoff actually look like? R produces what, in what format,
   landing where? Be concrete. This is the part people cannot find anywhere.
2. Why is "let the app compute it" the wrong instinct, and what goes wrong when
   someone does it?
3. When is Shiny the right answer and when does it stop being viable? Be fair to
   it. Scale, concurrency, hosting, integration with an existing site.
4. How do you keep it fast when the combination count is large? What is the
   modeling approach?
5. How does a new data cycle get published? This is the part that decides whether
   the thing is maintainable.
6. What does accessibility require here that a normal dashboard ignores? Section
   508 is not optional on federally funded work.

STRUCTURE SUGGESTION:
- The constraint first: the app renders, it does not compute. Explain why.
- The handoff. R output to queryable layer. Concrete.
- Shiny, honestly. When it works, when it does not.
- Performance and the combination problem.
- The annual cycle, and why it should be a data operation not a rebuild.
- Accessibility.
- Close by linking /services/health-data-platforms and the SRTR case study.

LENGTH: 700-750 words
AVOID:
- Anything covered by an NDA or specific to a client's internal pipeline.
- Dismissing Shiny. Plenty of readers are running it successfully.
- Generic data-viz advice. The value is the R boundary and the publish cycle.
-->
