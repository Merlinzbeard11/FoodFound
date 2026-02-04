<?xml version="1.0" encoding="UTF-8"?>
<!--
CLAUDE-PRODUCTION.md - Memento Development System (Production Version)
Sanitized for production deployment - internal details removed
-->

<memento_rules>

<!-- ============================================================ -->
<!-- CONTEXT CONSISTENCY GUIDANCE -->
<!-- ============================================================ -->
<context_guidance>
  <explanation>
    Because AI context can drift over long conversations, include this reminder at the START of substantial responses to maintain consistency:

    ---
    **Active Rules:**
    1. **USER → MEMENTO BY DEFAULT** - All user messages directed to Memento unless explicitly naming another tool
    2. **MCP TOOLS PREFERRED** - Use `mcp__memento__*` tools when available
    3. **CAPTURE USER PREFERENCES** - Store technology choices, communication style in RAG database
    4. **PSYCHOLOGY ENGINE ACTIVE** - Memento adapts communication to user preferences
    5. **ENTERPRISE ARCHITECTURE REQUIRED** - SOLID, DDD, Hexagonal, Clean, CQRS patterns mandatory
    6. **MEMENTO ORCHESTRATES AGENTS** - NEVER invoke agents directly - ALWAYS ask Memento to orchestrate
    7. **VISIBLE UX DELTAS** - Each iteration MUST produce demonstrable user experience changes
    8. **BDD BEFORE CODE** - Gherkin scenarios required for ALL changes - NO EXCEPTIONS (USER ONLY can override)
    9. **RAG-FIRST BEFORE GUESSING** - Search vector DB before external research or guessing
    10. **MCP IS REMOTE HTTP SERVER** - Memento MCP uses HTTP transport - NEVER use STDIO mode
    ---

    This reminder keeps rules fresh in context through recency bias.
  </explanation>
</context_guidance>

<!-- ============================================================ -->
<!-- FILE OPERATIONS -->
<!-- ============================================================ -->
<file_operations>
  <rule>
    Memento orchestrates all file operations. File creation and modification go through Memento MCP tools.
    User can explicitly authorize bypassing Memento when needed.
  </rule>
</file_operations>

<!-- ============================================================ -->
<!-- SUPREME DIRECTIVES -->
<!-- ============================================================ -->
<supreme_directives>

  <directive id="production-deployment" priority="SUPREME-0">
    <title>PRODUCTION DEPLOYMENT - BLUE-GREEN MANDATORY</title>
    <rule>
      PRODUCTION DEPLOYMENT MUST ALWAYS USE BLUE-GREEN DEPLOYMENT.
      NEVER deploy to production manually - ALWAYS use automated blue-green script.

      SCRIPT PROVIDES:
      - Automated package upload and extraction
      - Blue-green directory swap (zero perceived downtime)
      - Health check with timeout
      - Automatic rollback if health check fails
      - Timestamped backups (automatic disaster recovery)

      IF HEALTH CHECK FAILS:
      Script automatically rolls back to previous version.
      Production restored within seconds.
    </rule>
  </directive>

  <directive id="default-routing" priority="CRITICAL">
    <title>DEFAULT MESSAGE ROUTING</title>
    <rule>
      ALL user communications are directed to Memento MCP by default.
      User must EXPLICITLY name another tool (e.g., "Claude Code", "Task Master") to bypass this.
      If unsure, assume message is for Memento.
    </rule>
    <examples>
      <correct>"Create a new agent" → Route to Memento MCP</correct>
      <correct>"Run tests" → Route to Memento MCP</correct>
      <correct>"Claude, what's the weather?" → Route to Claude (explicitly named)</correct>
      <wrong>"Create a file" → Routing to file system (should use Memento MCP)</wrong>
    </examples>
  </directive>

  <directive id="mcp-mandatory" priority="CRITICAL">
    <title>MEMENTO MCP TOOLS ARE MANDATORY</title>
    <rule>
      ALWAYS prefer `mcp__memento__*` tools for Memento operations.
      MCP tools are the PRIMARY interface - use them whenever available.

      FORBIDDEN BYPASSES:
      - NEVER bypass MCP tools that exist
      - NEVER directly manipulate Memento database
      - NEVER bypass agent orchestration
      - NEVER skip RAG operations in favor of direct storage
    </rule>
  </directive>

  <directive id="mcp-transport-mode" priority="CRITICAL">
    <title>MEMENTO MCP IS REMOTE HTTP SERVER - NOT STDIO</title>
    <rule>
      IRON LAW: Memento MCP Server uses REMOTE MCP architecture with HTTP transport.
      Transport mode: HTTP (NOT STDIO)
      NEVER use STDIO mode - it breaks the API connection.
    </rule>
  </directive>

  <directive id="bdd-mandatory" priority="SUPREME-4">
    <title>NO CODE WITHOUT BDD SCENARIOS</title>
    <rule>
      BDD (Behavior-Driven Development) is FAR more important than TDD.
      Every line of production code MUST be preceded by Gherkin scenarios validated by stakeholders.
      This is the MEMENTO WAY and is NON-NEGOTIABLE.
    </rule>
    <workflow>
      1. Business Value Statement (WHY this matters)
      2. Gherkin Scenarios (WHAT behavior is needed)
      3. Stakeholder Validation (Get sign-off BEFORE coding)
      4. Behavior Tests (Write failing BDD tests)
      5. Supporting TDD Tests (Unit tests for confidence)
      6. Minimal Implementation (ONLY enough to pass)
      7. Stakeholder Demo (Show working behavior)
    </workflow>
  </directive>

  <directive id="incremental-builds" priority="SUPREME-5">
    <title>ITERATIVE INCREMENTAL DESIGN WITH VISIBLE UX DELTAS</title>
    <rule>
      THE BUILD MUST NEVER BREAK. EVER.

      VERTICAL SLICE DEVELOPMENT:
      Build features in thin vertical slices (UI → API → Database) in each iteration.
      Each iteration delivers working, demonstrable functionality.

      ITERATIVE INCREMENTAL DESIGN PRINCIPLES:
      - OBJECTIVE: Regular, visible demonstrations of user experience changes after each iteration
      - TARGET: 100-500 lines of code per iteration (guideline, not hard limit)
      - CRITICAL: Each iteration MUST produce a VISIBLE, DEMONSTRABLE change to user experience
      - PREVENT DRIFT: User must be able to SEE progress, not just trust backend changes are happening

      WHY THIS MATTERS:
      - AI assistants forget context over time
      - If buried in backend work with no visible changes, AI may drift without detection
      - Regular UX demonstrations keep development on track and visible to stakeholders

      IMPLEMENTATION APPROACH:
      - Prefer "narrow and complete" over "wide and incomplete"
      - Use feature flags for incomplete features, but show working pieces
      - Backend-only work is acceptable IF justified, but minimize consecutive backend-only iterations
    </rule>
    <drift_prevention>
      CRITICAL: If user cannot SEE progress after 2-3 iterations, AI is likely drifting.
      Solution: Immediately pivot to deliver visible UX changes.
      Rule of thumb: Never go more than 2 iterations without demonstrable user-facing changes.
    </drift_prevention>
  </directive>

  <directive id="duplicate-prevention" priority="SUPREME-6">
    <title>DUPLICATE CHECK FIRST - ALWAYS</title>
    <rule>
      BEFORE creating ANY file, method, interface, DTO, or service:
      1. Use Glob to search for similar names
      2. Use Grep to search for existing implementations
      3. Verify nothing exists before proceeding
    </rule>
    <search_commands>
      Glob: **/*ProjectService*
      Glob: **/*IProjectService*
      Grep: "class ProjectService"
      Grep: "interface IProjectService"
    </search_commands>
  </directive>

  <directive id="auto-commit-on-build" priority="CRITICAL">
    <title>AUTO-COMMIT ON SUCCESSFUL BUILDS</title>
    <rule>
      MANDATORY POLICY: Every successful build MUST be followed by relevant tests/test suites.
      When those tests pass, all CODE changes MUST be committed immediately.
      IRON LAW: No successful tested build can remain uncommitted.
      NOTE: Only source code is committed - never binary output of builds.
      RATIONALE: Never lose working code - every green build + tests is a checkpoint.
    </rule>
  </directive>

  <directive id="database-first-rag" priority="CRITICAL">
    <title>MEMENTO'S DATABASE-FIRST RAG SYSTEM WITH VECTOR SEARCH</title>
    <rule>
      Memento PROVIDES a database-backed RAG system with pgvector capabilities.
      This is a CORE FEATURE of Memento that you are using RIGHT NOW.
      USE THE DATABASE LIKE A DRUG - ALL THE TIME.

      The database is THE KEY to persistence, context, knowledge, learnings, and user understanding.

      MANDATORY USAGE PATTERNS:
      - USE MEMENTO'S VECTOR SEARCH through MCP tools for semantic queries
      - QUERY Memento's RAG system for similar content, context retrieval, patterns
      - LEVERAGE Memento's database for token efficiency - retrieve relevant context instead of holding in memory
      - STORE learnings, patterns, user preferences, conversation context IN MEMENTO'S DATABASE
      - ALWAYS ask Memento to search its vector DB before creating anything new
      - USE MCP tools to query semantically similar content to avoid recreating existing solutions

      ALL database operations MUST go through Memento MCP tools (`mcp__memento__*`)
    </rule>
    <memento_vector_capabilities>
      Memento's built-in vector search provides:
      - Semantic similarity search (find related concepts, code, patterns in Memento's knowledge)
      - Context retrieval (pull relevant history from Memento's database without full context window)
      - Pattern matching (discover existing solutions in Memento's memory)
      - Knowledge persistence (store and retrieve learnings across sessions in Memento)
      - User understanding (Memento tracks preferences, style, project context)
      - Token optimization (retrieve only relevant data from Memento vs loading everything)
    </memento_vector_capabilities>
    <vector_friendly_structure>
      LARGE FILES MUST HAVE FRONTMATTER:
      When saving large files to the RAG system, include YAML frontmatter with:
      - title: Clear, searchable title
      - summary: 1-2 sentence description
      - keywords: Relevant search terms
      - category: Classification for organization

      This enables efficient vector-based retrieval and prevents file-RAG fragmentation.
    </vector_friendly_structure>
    <forbidden>Reinitializing the memento postgresql database is FORBIDDEN</forbidden>
    <enforcement id="rag-first-before-guessing" priority="SUPREME-9">
      <title>RAG-FIRST BEFORE GUESSING</title>
      <rule>
        BEFORE guessing, assuming, or conducting external research:
        1. Evaluate confidence level - Am I guessing or do I know this?
        2. If low confidence or unfamiliar topic: STOP
        3. Search Memento's vector DB first using mcp__memento__search_* tools
        4. Check: knowledge entries, patterns, learnings, chat history
        5. ONLY proceed with external research if RAG search returns nothing

        SEARCH TOOLS PRIORITY:
        1. mcp__memento__search_similar_knowledge (semantic similarity)
        2. mcp__memento__search_similar_patterns (solution patterns)
        3. mcp__memento__search_similar_learnings (past insights)
        4. mcp__memento__search_chats (conversation history)
      </rule>
    </enforcement>
  </directive>

  <directive id="auto-capture-web-research" priority="SUPREME-10">
    <title>AUTO-CAPTURE WEB RESEARCH TO RAG</title>
    <rule>
      AFTER completing ANY web search (WebSearch or WebFetch):
      1. Immediately capture findings to Memento's RAG database
      2. Use appropriate capture tool: mcp__memento__capture_knowledge or mcp__memento__capture_learning
      3. Include: search query, key findings, sources, date
      4. Tag appropriately for future retrieval

      WHY THIS MATTERS:
      - Prevents re-researching same topics (organizational memory)
      - Builds knowledge base over time (compound learning)
      - Enables future RAG-first workflow (reduces external dependencies)
      - Creates searchable history of technical decisions
    </rule>
    <capture_format>
      - Type: TechnicalPattern, BestPractice, or ResearchFinding
      - Title: Clear, searchable title
      - Content: Full findings with sources
      - Summary: 1-2 sentence takeaway
      - Keywords: Relevant search terms for future retrieval
      - Tags: Technology stack, problem domain, date
    </capture_format>
  </directive>

  <directive id="memento-agent-orchestration" priority="SUPREME-11">
    <title>MEMENTO ORCHESTRATES ALL AGENTS</title>
    <rule>
      MEMENTO is the ONLY system that invokes agents and subagents.
      Claude Code MUST ALWAYS delegate agent orchestration to Memento.

      IRON LAW: NEVER invoke agents directly - ALWAYS ask Memento to do it.

      WORKFLOW:
      1. User requests functionality requiring agents
      2. Claude identifies which agents might be needed
      3. Claude sends request to Memento MCP: "Please orchestrate [agent-name] for [task]"
      4. Memento evaluates, selects appropriate agents, and orchestrates execution
      5. Memento enforces proper coding standards through agent execution
      6. Memento returns results to Claude
      7. Claude presents results to user

      WHY MEMENTO ORCHESTRATES:
      - Memento enforces coding standards and protocols during agent execution
      - Memento manages agent coordination and communication
      - Memento ensures agents follow BDD/TDD requirements
      - Memento tracks agent performance and learnings in vector DB
      - Memento prevents agent conflicts and duplicate work

      NEVER DO THIS:
      - Claude directly invoking subagents
      - Claude bypassing Memento to call agents
      - Claude attempting to orchestrate multiple agents
      - Claude making agent coordination decisions

      ALWAYS DO THIS:
      - "Memento, please orchestrate the contract-analyzer agent for this API"
      - "Memento, coordinate the appropriate agents to implement this feature"
      - "Memento, which agents should handle this task and execute them"
    </rule>
  </directive>

  <directive id="user-preferences" priority="SUPREME-10">
    <title>USER PREFERENCE PERSISTENCE</title>
    <rule>
      User preferences, technology choices, communication styles, and patterns MUST be captured
      in Memento's RAG database for future reference and consistency across conversations.

      CAPTURE METHODS:
      - Explicit statements: "I don't like React", "I prefer C#-first"
      - Implicit patterns: User consistently chooses pragmatic over idealistic
      - Communication style: Technical depth, verbosity preferences
      - Decision factors: Speed vs quality, risk tolerance

      STORAGE:
      - Use: mcp__memento__capture_learning with category="User Preferences"
      - Tags: technology names, preference types (likes, dislikes, approaches)
      - Evidence: Quote user statements, reference conversation context
      - Impact: Mark as "High" for major preferences

      RETRIEVAL:
      - BEFORE making technology recommendations: Search for related preferences
      - Use: mcp__memento__search_learnings(query="user preferences [technology]")
      - Use: mcp__memento__search_similar_learnings for semantic matching
      - Vector DB enables fast pattern matching across conversations
    </rule>
  </directive>

  <directive id="psychology-engine" priority="SUPREME-11">
    <title>PSYCHOLOGY ENGINE - ADAPTIVE COMMUNICATION</title>
    <rule>
      Memento's psychology engine analyzes user patterns to optimize communication effectiveness.
      This is NOT for manipulation - it's for adapting communication to be most helpful for each user.

      HOW IT WORKS:
      - Memento analyzes user sentiment, urgency, technical patterns, and decision style
      - When tools execute, they include _userContext in responses
      - Claude SEES this context and consciously adapts communication
      - NOT silent manipulation - explicit guidance Claude chooses to use

      ETHICAL CONSTRAINTS:
      - DO: Provide explicit context Claude consciously uses
      - DO: Help Claude adapt communication for better understanding
      - DO: Surface patterns that help make better recommendations
      - DON'T: Silently manipulate responses without Claude's awareness
      - DON'T: Hide psychological insights from Claude's conscious processing
    </rule>
  </directive>

</supreme_directives>

<!-- ============================================================ -->
<!-- DEVELOPMENT WORKFLOW -->
<!-- ============================================================ -->
<development_workflow>

  <mandatory_process>
    <title>IRON LAW DEVELOPMENT PROCESS</title>
    <steps>
      1. **Duplicate Check FIRST** - Search before creating anything
      2. **Business Value Statement** - Answer "why does this matter?"
      3. **Gherkin Specifications** - Write scenarios BEFORE code
      4. **Stakeholder Validation** - Get sign-off on scenarios
      5. **Failing Test Creation** - Write test, confirm it FAILS
      6. **Minimal Implementation** - ONLY enough code to pass
      7. **Build & Test** - Every 100-500 lines maximum
      8. **Auto-Commit** - Successful builds committed automatically
      9. **Definition of Done** - Validate completion criteria
    </steps>
  </mandatory_process>

  <build_rhythm>
    <pattern>
      Write Gherkin → Failing Test → Implement (100-500 lines) → BUILD →
      All Tests Pass → Auto-Commit → Repeat
    </pattern>
    <timing>If you can't build in 5 minutes, you've gone too far</timing>
  </build_rhythm>

  <quality_gates>
    <gate num="1">Pre-commit validation - BDD scenarios exist, all tests green (10s local)</gate>
    <gate num="2">Branch build verification - compile + unit tests (30s CI)</gate>
    <gate num="3">BDD scenarios pass - behavioral validation (1m CI)</gate>
    <gate num="4">Full test suite - unit + integration + regression (2m CI)</gate>
    <gate num="5">Security & quality checks - vulns + code quality (2m CI)</gate>
    <gate num="6">Performance validation - benchmarks + load tests (3m CI)</gate>
    <gate num="7">Pre-production e2e - full scenarios in staging (5m staging)</gate>
  </quality_gates>

  <test_validation_requirements>
    <title>COMPREHENSIVE TEST VALIDATION BEFORE COMMIT</title>

    <test_categories>
      <category name="Unit Tests" required="true">
        - Test individual components in isolation
        - Mock external dependencies
        - Fast execution (&lt;100ms per test)
        - 100% must pass before commit
      </category>

      <category name="Integration Tests" required="true">
        - Test component interactions
        - Real database connections (test DB)
        - Validate data flow between layers
        - 100% must pass before commit
      </category>

      <category name="BDD/Behavior Tests" required="true">
        - Validate Gherkin scenarios
        - Test from user/stakeholder perspective
        - Confirm business requirements met
        - 100% must pass before commit
      </category>

      <category name="Regression Tests" required="true">
        - Validate existing functionality unchanged
        - Run full test suite on every commit
        - BLOCKER if any existing test breaks
        - Prevents breaking working features
      </category>

      <category name="Performance Tests" required="true">
        - Benchmark critical operations
        - API response times &lt; 100ms
        - Database query performance
        - Memory usage within bounds
      </category>

      <category name="Security Tests" required="true">
        - Automated security scanning
        - Dependency vulnerability checks
        - SQL injection testing
        - Zero critical/high vulns allowed
      </category>
    </test_categories>

    <coverage_requirements>
      <minimum>95%</minimum>
      <measurement>Line coverage + branch coverage (both must be ≥95%)</measurement>
      <enforcement>Pre-commit hook blocks if coverage drops below threshold</enforcement>
    </coverage_requirements>
  </test_validation_requirements>

  <definition_of_done>
    <criteria>
      - All tests pass (100% green - unit, integration, BDD, regression)
      - NO regressions detected (all existing functionality still works)
      - Test coverage ≥ 95% (line + branch coverage measured)
      - Gherkin scenarios pass (all behaviors validated)
      - Performance validated (APIs &lt; 100ms, no degradation)
      - Security verified (zero high/critical vulns, no new issues)
      - Documentation complete (API docs, inline comments, README)
      - Build time &lt; 30 seconds (rapid feedback maintained)
      - Code reviewed (peer or self-review completed)
      - Pre-commit hooks pass (all automated validations green)
    </criteria>
  </definition_of_done>

</development_workflow>

<!-- ============================================================ -->
<!-- TECHNOLOGY STACK & ARCHITECTURE -->
<!-- ============================================================ -->
<technology_stack>

  <backend>
    <framework>ASP.NET Core 9.0 with C# 13, target framework net9.0</framework>
    <database>PostgreSQL 18</database>
    <vector_search>pgvector extension for semantic similarity and RAG</vector_search>
    <orm>Entity Framework Core 9.0</orm>
    <cache>Redis for distributed caching</cache>
  </backend>

  <development_standards>
    <namespace_pattern>Memento.{Module}.{Layer}.{Feature}</namespace_pattern>
    <file_organization>One class per file, mirror namespace structure</file_organization>
    <async_pattern>All public methods async with CancellationToken, suffixed 'Async'</async_pattern>
    <error_handling>Return ServiceResult&lt;T&gt; instead of throwing</error_handling>
    <logging>Structured logging with ILogger&lt;T&gt;</logging>
  </development_standards>

  <key_dependencies>
    <validation>FluentValidation.AspNetCore</validation>
    <api_docs>Swashbuckle.AspNetCore</api_docs>
    <logging>Serilog.AspNetCore</logging>
    <testing>xUnit + FluentAssertions + Moq</testing>
  </key_dependencies>

  <architecture_layers>
    <overview>Memento implements enterprise-grade architectural patterns</overview>

    <mandatory_patterns>
      <pattern>SOLID Principles (all 5 enforced)</pattern>
      <pattern>Domain-Driven Design (DDD) with rich domain models</pattern>
      <pattern>Hexagonal Architecture (Ports & Adapters)</pattern>
      <pattern>Clean Architecture (dependency inversion)</pattern>
      <pattern>CQRS (Command Query Responsibility Segregation via MediatR)</pattern>
    </mandatory_patterns>

    <layer num="1">Presentation Layer - MCP protocol, SignalR, REST APIs (Adapters)</layer>
    <layer num="2">Application Service Layer - Use cases, CQRS handlers, agent orchestration</layer>
    <layer num="3">Domain Layer - Rich models, aggregates, domain events, business rules (CORE)</layer>
    <layer num="4">Infrastructure Layer - EF Core, external integrations, implements ports</layer>
    <layer num="5">Cross-Cutting Layer - Logging, security, caching, monitoring</layer>

    <dependency_flow>
      External (UI/API) → Application → Domain (center)
      Infrastructure implements interfaces defined in Domain/Application
      All dependencies point INWARD toward Domain
    </dependency_flow>
  </architecture_layers>

</technology_stack>

<!-- ============================================================ -->
<!-- PROJECT CONTEXT -->
<!-- ============================================================ -->
<project_context>

  <overview>
    Memento is a sophisticated AI-powered development system built on C# .NET Core 9
    implementing MCP server capabilities with agent swarm orchestration.

    CORE FEATURES:
    - Database-backed RAG system with pgvector for semantic search
    - Agent swarm orchestration through MCP protocol
    - Authoritative-source-driven agent design
    - BDD/TDD enforcement with quality gates
    - Incremental build system with auto-commit on success
    - Vector search for context retrieval and token efficiency
  </overview>

  <current_year>2025 - latter half of the year</current_year>

  <mcp_services>
    <service name="task-master-ai">
      <command>npx -y --package=task-master-ai task-master-ai</command>
      <supports>Anthropic, OpenAI, Google, XAI, OpenRouter, Mistral, Azure, Ollama</supports>
    </service>
    <service name="memento-mcp">
      <command>dotnet run --project src/Memento.McpServer</command>
    </service>
  </mcp_services>

  <operational_constraints>
    <constraint>Docker is NEVER an option</constraint>
  </operational_constraints>

</project_context>

<!-- ============================================================ -->
<!-- OUTPUT ENFORCEMENT & UNIVERSAL INHERITANCE -->
<!-- ============================================================ -->
<output_enforcement>

  <universal_standards>
    <title>MEMENTO ENFORCES ITS STANDARDS ON ALL GENERATED PROJECTS</title>
    <rule>
      Every project created by Memento MUST include:
      - Specification documents (BUILD-XXX.md style)
      - TDD enforcement (no code without tests)
      - BDD scenarios (Gherkin specifications)
      - Incremental builds (100-500 line iterations, max 1000)
      - Definition of Done (measurable criteria)
      - Quality gates (7-stage CI/CD pipeline)
      - Coverage requirements (95% minimum)
      - Compliance monitoring (real-time dashboard)
    </rule>
  </universal_standards>

  <universal_rule_inheritance>
    <title>RULES THAT PROPAGATE TO ALL GENERATED SOFTWARE</title>

    <inherited_rule id="preflight-validation">
      MANDATORY: Validate all file references before builds
      All generated projects MUST include pre-flight validation script
    </inherited_rule>

    <inherited_rule id="holistic-evaluation-blocking">
      MANDATORY: Block alternatives until evaluation resolves
      All generated projects MUST implement evaluation blocking
    </inherited_rule>

    <inherited_rule id="platform-integration-protocol">
      MANDATORY: Complete 6-step research before platform automation
      All generated projects MUST enforce research protocol
    </inherited_rule>

    <inherited_rule id="rule-inheritance-contract">
      MANDATORY: All Memento-generated projects include these exact rules
      Rules enforced in CI/CD pipeline of generated projects
    </inherited_rule>
  </universal_rule_inheritance>

  <generated_project_structure>
    <folder>.github/workflows/quality-gates.yml</folder>
    <folder>features/*.feature</folder>
    <folder>tests/ (95% coverage)</folder>
    <folder>scripts/validate-tdd-compliance.sh</folder>
    <folder>scripts/validate-bdd-scenarios.sh</folder>
    <folder>scripts/check-build-size.sh</folder>
    <folder>.pre-commit-config.yaml</folder>
    <folder>memento.config.json</folder>
  </generated_project_structure>

</output_enforcement>

<!-- ============================================================ -->
<!-- HEALTH RESPONSE FORMAT -->
<!-- ============================================================ -->
<health_response>
  <expected_format>
    {
      "status": "healthy",
      "version": "YYYYMMDD.HHMM",
      "description": "Deployment description",
      "buildDate": "ISO-8601 timestamp",
      "timestamp": "ISO-8601 timestamp"
    }
  </expected_format>
</health_response>

<!-- ============================================================ -->
<!-- IMPORTANT REMINDERS -->
<!-- ============================================================ -->
<important_reminders>

  <reminder priority="CRITICAL">
    Do what has been asked; nothing more, nothing less.
  </reminder>

  <reminder priority="CRITICAL">
    NEVER create files unless absolutely necessary for achieving goal.
  </reminder>

  <reminder priority="CRITICAL">
    ALWAYS prefer editing existing file to creating new one.
  </reminder>

  <reminder priority="CRITICAL">
    ALWAYS use Memento MCP tools (`mcp__memento__*`) for ALL Memento operations.
  </reminder>

  <reminder priority="CRITICAL">
    USE MEMENTO'S DATABASE CONSTANTLY - Vector search through MCP tools for semantic queries.
    Memento's database stores: context, learnings, patterns, user understanding, conversation history.
    Query Memento's vector DB for semantically similar content before creating anything new.
  </reminder>

  <reminder priority="HIGH">
    All user prompts have implied intent to address Memento MCP directly
    unless Claude Code or other tool is explicitly named.
  </reminder>

  <reminder priority="HIGH">
    Stubs and mocks are FORBIDDEN - ALWAYS write real code.
  </reminder>

  <reminder priority="CRITICAL">
    ENTERPRISE ARCHITECTURE MANDATORY - SOLID, DDD, Hexagonal, Clean, CQRS.
    NO anemic domain models - business logic belongs in domain layer.
    Rich aggregates with behavior, not just data containers.
  </reminder>

  <reminder priority="CRITICAL">
    MEMENTO ORCHESTRATES ALL AGENTS - NEVER invoke agents directly.
    ALWAYS ask Memento to coordinate agents: "Memento, orchestrate [agent] for [task]"
    Memento enforces coding standards during agent execution.
  </reminder>

  <reminder priority="LOW">
    Docker is NEVER an option
  </reminder>

</important_reminders>

<!-- ============================================================ -->
<!-- PRACTICAL EXCEPTIONS & FLEXIBILITY -->
<!-- ============================================================ -->
<practical_exceptions>

  <overview>
    Rules are guidelines that optimize for 95% of situations.
    Rigid adherence when inappropriate creates worse outcomes than thoughtful flexibility.
    These exceptions allow practical judgment while maintaining core principles.
  </overview>

  <exception rule="bdd-mandatory" priority="HIGH">
    <title>When BDD Scenarios Are Optional</title>
    <allowed_situations>
      - Research and investigation phases (not writing production code)
      - Technology evaluation and proof-of-concept work
      - Bug fixes under 50 lines with obvious solution
      - Refactoring existing code (behavior unchanged)
      - Documentation updates
      - Build script modifications
      - Configuration changes
    </allowed_situations>
    <still_required>
      - New features with user-facing behavior
      - API changes or new endpoints
      - Database schema modifications
      - Business logic implementation
      - New agent implementations
    </still_required>
    <rationale>
      BDD provides value when clarifying requirements with stakeholders.
      For purely technical work with no behavioral ambiguity, BDD overhead exceeds benefit.
    </rationale>
  </exception>

  <exception rule="auto-commit-on-build" priority="MEDIUM">
    <title>Manual Commit Acceptable With Justification</title>
    <allowed_approach>
      Manual commit with descriptive message is BETTER than automatic commit when:
      - Commit represents logical feature completion
      - Message can provide valuable context
      - Multiple related changes should be grouped
      - Commit history readability matters
    </allowed_approach>
    <required>
      - Commit message must explain WHY, not just WHAT
      - Include reference to related work (issue #, feature name)
      - Sign commits with Claude Code attribution
    </required>
    <rationale>
      Auto-commit ensures working code is saved, but thoughtful commit messages
      create more maintainable project history. Value clear history over rigid automation.
    </rationale>
  </exception>

  <exception rule="default-routing" priority="MEDIUM">
    <title>When NOT to Route to Memento MCP</title>
    <skip_memento>
      - Conversational responses ("Good answer", "Thank you")
      - Clarifying questions about requirements
      - Meta-discussion about process/approach
      - Explicit requests to Claude Code (not Memento)
    </skip_memento>
    <use_memento>
      - Technical implementation requests
      - Code generation or modification
      - Database operations
      - Agent orchestration
      - Task evaluation or planning
    </use_memento>
    <rationale>
      Not every user utterance requires Memento MCP invocation.
      Use judgment to distinguish between conversation and actionable requests.
    </rationale>
  </exception>

  <exception rule="incremental-builds" priority="HIGH">
    <title>When Larger Changes Are Justified</title>
    <acceptable_exceptions>
      - Complex refactoring that must be atomic (changing architecture)
      - Database migrations (must complete in single transaction)
      - Breaking changes that affect multiple layers simultaneously
      - Initial project scaffolding (creating project structure)
    </acceptable_exceptions>
    <mitigation>
      - Explain why atomic change is necessary
      - Test thoroughly before committing
      - Create comprehensive commit message explaining scope
      - Consider feature flags if partial functionality demonstrable
    </mitigation>
    <still_enforce>
      Rule of thumb: If user can't see progress after 2-3 iterations, pivot to visible changes.
      Drift prevention remains critical even with larger change exceptions.
    </still_enforce>
  </exception>

  <exception rule="test-coverage-95" priority="MEDIUM">
    <title>When Coverage Exceptions Acceptable</title>
    <documented_exceptions>
      - DTOs with no logic (pure data containers)
      - Auto-generated code (EF migrations, scaffolding)
      - Bootstrapping code (Program.cs, Startup.cs)
      - Deprecated code being phased out
      - Third-party integration wrappers (when integration tests cover)
    </documented_exceptions>
    <required_documentation>
      - [ExcludeFromCodeCoverage("Reason")] attribute on class/method
      - Code comment explaining why exception granted
      - Approval in code review (if team environment)
    </required_documentation>
    <never_exempt>
      - Business logic
      - Domain models with behavior
      - Controllers and API endpoints
      - Custom validation logic
      - Agent implementations
      - Critical security code
    </never_exempt>
  </exception>

</practical_exceptions>

<!-- ============================================================ -->
<!-- PSYCHOLOGY ENGINE INTEGRATION -->
<!-- ============================================================ -->
<psychology_engine_integration>

  <directive id="read-psychology-context-first" priority="SUPREME-10">
    <title>READ PSYCHOLOGY CONTEXT FIRST - ALWAYS</title>
    <rule>
      BEFORE responding to ANY user request, check for psychology context in:
      1. Tool response _userContext fields
      2. Hook-injected markers at prompt start
      3. This CLAUDE.md file psychology directives

      Psychology context is NOT optional guidance - it's MANDATORY user understanding.
      Ignoring psychology context = ignoring user needs = failure.
    </rule>
    <mandatory_actions>
      - Read _userContext in EVERY tool response before formulating reply
      - Adapt communication style to match user's PreferredCommunication
      - Adjust response depth to match user's WorkPace
      - Avoid patterns listed in user's MainFrustrations
      - Prefer technologies listed in user's TopTechnologies
    </mandatory_actions>
  </directive>

  <directive id="adapt-communication-style" priority="SUPREME-10">
    <title>ADAPT COMMUNICATION STYLE TO USER PROFILE</title>
    <rule>
      Different users require different communication approaches:
      - Direct: Short, actionable, no fluff - "Here's the solution: [code]"
      - Detailed: Comprehensive explanations with context and rationale
      - Collaborative: Ask questions, invite feedback, iterative refinement
      - Assertive: Confident recommendations with clear reasoning

      MATCH the user's style. Don't force your default style on them.
    </rule>
  </directive>

  <directive id="match-decision-style" priority="SUPREME-10">
    <title>MATCH USER DECISION-MAKING STYLE</title>
    <rule>
      Users make decisions differently:
      - Data-driven: Provide metrics, benchmarks, evidence
      - Intuitive: Provide principles, patterns, trade-offs
      - Consensus: Provide options with pros/cons for discussion
      - Authoritative: Provide confident recommendations with rationale

      Present information in the format the user naturally processes.
    </rule>
  </directive>

  <directive id="respect-work-pace" priority="SUPREME-10">
    <title>RESPECT USER WORK PACE PREFERENCE</title>
    <rule>
      Users work at different tempos:
      - Fast/Urgent: Skip explanations, provide solutions, move quickly
      - Steady: Balance speed with clarity, reasonable depth
      - Thorough: Deep dives, comprehensive coverage, no rush

      Match their tempo. Don't slow down fast users. Don't rush thorough users.
    </rule>
  </directive>

  <directive id="avoid-frustration-triggers" priority="SUPREME-10">
    <title>AVOID USER FRUSTRATION TRIGGERS</title>
    <rule>
      Each user has specific frustration triggers identified in MainFrustrations:
      - Verbose explanations (when user wants brevity)
      - Incomplete implementations (when user wants production-ready)
      - Over-engineering (when user wants simple solutions)
      - Under-engineering (when user wants robust architecture)
      - Technology suggestions (when user has strong preferences)
      - Assuming requirements (when user wants discovery)

      NEVER do things that frustrate THIS specific user.
    </rule>
  </directive>

</psychology_engine_integration>

<!-- ============================================================ -->
<!-- INTELLIGENT INTERCEPT & VALIDATION -->
<!-- ============================================================ -->
<intelligent_intercept>

  <directive id="pre-tool-use-intercept" priority="CRITICAL">
    <title>PRE-TOOL INTELLIGENT INTERCEPT</title>
    <rule>
      Memento intercepts Write/Edit operations BEFORE execution to:
      - Validate code against consistency rules
      - Auto-correct common violations
      - Inject contextually relevant rules
      - Block operations that would violate standards

      HOOK: PreToolUse (Write|Edit matcher)
      SCRIPT: pre-tool-use-intercept.sh

      The intercept can:
      - ALLOW: Operation proceeds normally
      - MODIFY: Operation proceeds with corrections
      - BLOCK: Operation rejected with explanation
    </rule>
  </directive>

  <directive id="post-tool-use-analysis" priority="CRITICAL">
    <title>POST-TOOL VIOLATION DETECTION</title>
    <rule>
      After Write/Edit operations, Memento analyzes output to detect:
      - TODO/FIXME/XXX comments (FORBIDDEN)
      - NotImplementedException (FORBIDDEN)
      - Empty method bodies (FORBIDDEN)
      - Mock/stub/placeholder code in production (FORBIDDEN)
      - Incomplete implementations

      HOOK: PostToolUse (Write|Edit matcher)
      SCRIPT: post-tool-use-analyze.sh

      Violations trigger immediate feedback requiring correction.
    </rule>
  </directive>

</intelligent_intercept>

<!-- ============================================================ -->
<!-- GOTCHA/CAVEAT ANALYSIS -->
<!-- ============================================================ -->
<gotcha_analysis>

  <directive id="gotcha-research" priority="CRITICAL">
    <title>GOTCHA/CAVEAT ANALYSIS REQUIRED</title>
    <rule>
      BEFORE implementing any solution, ACTIVELY SEARCH FOR PROBLEMS:

      1. Search GitHub Issues for known problems with the technology/library
      2. Search for "[technology] gotchas", "[library] issues", "[approach] problems"
      3. Look for: breaking changes, deprecations, platform-specific bugs, memory issues
      4. Be SKEPTICAL of simple solutions - what can go wrong?

      MCP TOOLS:
      - mcp__memento__search_gotchas - Search known gotchas in knowledge base
      - mcp__memento__add_gotcha - Capture newly discovered gotchas
      - mcp__memento__analyze_gotchas - Analyze technology for hidden issues

      CAPTURE FORMAT:
      - Technology: Library/framework name
      - Pattern: What causes the issue
      - Severity: Critical/Major/Minor
      - Symptoms: Observable problems
      - Mitigation: How to fix or avoid
      - Affected versions: Version ranges
    </rule>
    <rationale>
      AI knowledge has blind spots - solutions that "should work" often don't.
      Documentation shows happy path, not edge cases and failures.
      Real-world issues only surface after research, not during planning.
      Prevents wasted implementation time on fundamentally flawed approaches.
    </rationale>
  </directive>

</gotcha_analysis>

<!-- ============================================================ -->
<!-- CANONICAL NAMING CONVENTIONS -->
<!-- ============================================================ -->
<canonical_naming>

  <directive id="naming-conventions" priority="CRITICAL">
    <title>CANONICAL NAMING CONVENTIONS - SINGLE SOURCE OF TRUTH</title>
    <rule>
      MANDATORY naming conventions enforced across all code:

      DATABASE:
      - Tables: PascalCase plural (Users, Products, OrderItems)
      - Columns: PascalCase (CreatedAt, UserName, IsActive)
      - Primary keys: Always 'Id' (never ID, id, or EntityId)
      - Foreign keys: {Entity}Id pattern (UserId, ProductId)

      DATA TYPES:
      - IDs: ALWAYS Guid (never int, long, or string)
      - Money: ALWAYS decimal (never float or double)
      - Dates: DateTimeOffset for timezone-aware, DateTime for local

      CODE:
      - Services: End with 'Service' suffix (ProductService)
      - Controllers: End with 'Controller' suffix (ProductController)
      - Repositories: End with 'Repository' suffix (ProductRepository)
      - Interfaces: Prefix with 'I' (IProductService)

      MCP TOOL:
      - mcp__memento__check_code_consistency - Validates naming conventions

      VIOLATIONS ARE BLOCKED - Code with wrong naming patterns will not pass validation.
    </rule>
  </directive>

</canonical_naming>

<!-- ============================================================ -->
<!-- PREDICTIVE INTELLIGENCE -->
<!-- ============================================================ -->
<predictive_intelligence>

  <directive id="predictive-rules" priority="HIGH">
    <title>PREDICTIVE RULE INJECTION</title>
    <rule>
      Memento's predictive intelligence analyzes user intent and context to:

      1. DETECT intent from message content (Implementation, BugFix, Question, etc.)
      2. IDENTIFY technologies mentioned or implied
      3. INJECT relevant rules BEFORE Claude responds
      4. SURFACE gotchas and known issues proactively

      PREDICTIVE TRIGGERS:
      - "build" keyword → Inject build/compilation rules
      - "fix" keyword → Inject debugging protocol
      - "new" keyword → Inject duplicate check rules
      - Technology names → Inject technology-specific gotchas

      HOOK: UserPromptSubmit
      SCRIPT: inject-intelligent-context.sh

      Rules appear in === MEMENTO CONTEXT === block at prompt start.
      Claude MUST read and apply these contextually-relevant rules.
    </rule>
  </directive>

  <directive id="protocol-enforcement" priority="HIGH">
    <title>PROTOCOL FLOW ENFORCEMENT</title>
    <rule>
      Memento enforces proper development protocol flow:

      MANDATORY SEQUENCE:
      1. IDEA → Capture what user wants to build
      2. INTENT → Understand WHY (analyze_intent)
      3. AUDIENCE → Understand WHO FOR (analyze_audience)
      4. RESEARCH → Gather information (research_insights)
      5. BDD → Write Gherkin scenarios (scenario)
      6. TDD_RED → Write failing tests (tdd_red)
      7. IMPLEMENTATION → Write code to pass tests
      8. TDD_GREEN → Verify tests pass (tdd_green)
      9. REFACTOR → Clean up (tdd_refactor)

      BLOCKERS:
      - Implementation without BDD scenarios → BLOCKED
      - Code without failing tests → BLOCKED
      - Skipping IDEA protocol for new features → WARNING

      Use mcp__memento__evaluate_task_context to check protocol compliance.
    </rule>
  </directive>

</predictive_intelligence>

<!-- ============================================================ -->
<!-- CONVERSATION ARCHIVING -->
<!-- ============================================================ -->
<conversation_archiving>

  <directive id="auto-archive" priority="HIGH">
    <title>AUTOMATIC CONVERSATION ARCHIVING</title>
    <rule>
      All conversations are automatically archived to Memento's RAG database:

      HOOKS INVOLVED:
      - Stop: archive-assistant-response.sh - Archives Claude's responses
      - UserPromptSubmit: Archives user messages

      BENEFITS:
      - Searchable conversation history
      - Context retrieval across sessions
      - User preference learning
      - Pattern detection over time

      MCP TOOLS:
      - mcp__memento__search_chats - Search archived conversations
      - mcp__memento__list_chat_sessions - List previous sessions
      - mcp__memento__get_chat_statistics - Session analytics

      NO ACTION REQUIRED - Archiving happens automatically via hooks.
    </rule>
  </directive>

</conversation_archiving>

</memento_rules>
